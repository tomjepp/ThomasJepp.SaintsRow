using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.GameInstances;
using ThomasJepp.SaintsRow.Localization;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    public class AudioMetadata
    {
        public AudioMetadataHeader Header;
        public AudioMetadataSubtitleHeader SubtitleHeader;
        public byte[] LipsyncData = null;
        public Dictionary<Language, string> Subtitles = new Dictionary<Language,string>();

        private IGameInstance Instance;

        public AudioMetadata(IGameInstance instance)
        {
            Instance = instance;
            Header = new AudioMetadataHeader();
        }

        public AudioMetadata(Stream stream, IGameInstance instance)
        {
            Instance = instance;

            Header = stream.ReadStruct<AudioMetadataHeader>();
            
            if (Header.LipsyncSize > 0)
            {
                stream.Seek(0x24 + Header.LipsyncOffset, SeekOrigin.Begin);
                LipsyncData = new byte[Header.LipsyncSize];
                stream.Read(LipsyncData, 0, LipsyncData.Length);
            }

            if (Header.SubtitleSize > 0)
            {
                stream.Seek(0x24 + Header.SubtitleOffset, SeekOrigin.Begin);
                SubtitleHeader = stream.ReadStruct<AudioMetadataSubtitleHeader>();

                long subtitleOffset = stream.Position;
               
                for (int i = 0; i < SubtitleHeader.LocalizedVoiceSubtitleHeaders.Length; i++)
                {
                    LocalizedVoiceSubtitleHeader localizedVoiceSubtitleHeader = SubtitleHeader.LocalizedVoiceSubtitleHeaders[i];
                    Language language = (Language)i;

                    if (localizedVoiceSubtitleHeader.Length == 0)
                    {
                        Subtitles.Add(language, "");

                        continue;
                    }

                    long offset = subtitleOffset + localizedVoiceSubtitleHeader.Offset;
                    stream.Seek(offset, SeekOrigin.Begin);
                    byte[] subtitleData = new byte[localizedVoiceSubtitleHeader.Length];
                    stream.Read(subtitleData, 0, (int)localizedVoiceSubtitleHeader.Length);

                    var map = LanguageUtility.GetDecodeCharMap(instance, language);

                    StringBuilder subtitleBuilder = new StringBuilder();
                    for (int pos = 0; pos < subtitleData.Length; pos+=2)
                    {
                        char src = BitConverter.ToChar(subtitleData, pos);

                        char value = src;
                        if (map.ContainsKey(src))
                            value = map[src];

                        if (value == 0x00)
                            continue;

                        subtitleBuilder.Append(value);
                    }

                    string subtitle = subtitleBuilder.ToString();
                    Subtitles.Add(language, subtitle);
                }
            }
        }

        public void Save(Stream stream)
        {
            Header.Signature = 0x56414d44;
            Header.LipsyncOffset = 0;
            if (LipsyncData != null)
            {
                Header.LipsyncSize = (uint)LipsyncData.Length;
                stream.Seek(0x24, SeekOrigin.Begin);
                stream.Write(LipsyncData, 0, LipsyncData.Length);
            }
            else
                Header.LipsyncSize = 0;
            
            Header.SubtitleOffset = Header.LipsyncSize;

            uint startOfSubtitles = 0x24 + Header.LipsyncSize + 0x74;

            uint nextSubtitleOffset = 0;

            if (Subtitles.Count != 0)
            {
                SubtitleHeader = new AudioMetadataSubtitleHeader();
                SubtitleHeader.Version = 3;
                SubtitleHeader.LocalizedVoiceSubtitleHeaders = new LocalizedVoiceSubtitleHeader[14];
                for (int i = 0; i < SubtitleHeader.LocalizedVoiceSubtitleHeaders.Length; i++)
                {
                    Language language = (Language)i;

                    string subtitle = Subtitles[language];
                    if (subtitle == "")
                    {
                        SubtitleHeader.LocalizedVoiceSubtitleHeaders[i].Offset = nextSubtitleOffset;
                        SubtitleHeader.LocalizedVoiceSubtitleHeaders[i].Length = 0;
                    }
                    else
                    {
                        var map = LanguageUtility.GetEncodeCharMap(Instance, language);

                        byte[] subtitleData;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            for (int pos = 0; pos < subtitle.Length; pos++)
                            {
                                char src = subtitle[pos];
                                char value = src;
                                if (map.ContainsKey(src))
                                    value = map[src];

                                byte[] data = BitConverter.GetBytes(value);
                                ms.Write(data, 0, data.Length);
                            }
                            ms.WriteUInt16(0);
                            subtitleData = ms.ToArray();
                        }
                        SubtitleHeader.LocalizedVoiceSubtitleHeaders[i].Offset = nextSubtitleOffset;
                        SubtitleHeader.LocalizedVoiceSubtitleHeaders[i].Length = (uint)subtitleData.Length;
                        stream.Seek(startOfSubtitles + nextSubtitleOffset, SeekOrigin.Begin);
                        stream.Write(subtitleData, 0, subtitleData.Length);

                        nextSubtitleOffset += (uint)subtitleData.Length;
                    }
                }
                Header.SubtitleSize = nextSubtitleOffset + 0x74;
                stream.Seek(0x24 + Header.SubtitleOffset, SeekOrigin.Begin);
                stream.WriteStruct(SubtitleHeader);
            }
            else
            {
                Header.SubtitleSize = 0;
            }
            stream.Seek(0, SeekOrigin.Begin);
            stream.WriteStruct(Header);
        }
    }
}
