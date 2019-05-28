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
        public uint SubtitleVersion;

        public byte[] LipsyncData = null;
        public Dictionary<Language, string> Subtitles = new Dictionary<Language, string>();
        public Dictionary<Language, string> MaleSubtitles = new Dictionary<Language, string>();
        public Dictionary<Language, string> FemaleSubtitles = new Dictionary<Language, string>();

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
                SubtitleVersion = stream.ReadUInt32();

                stream.Seek(0x24 + Header.SubtitleOffset, SeekOrigin.Begin);

                switch (SubtitleVersion)
                {
                    case 2:
                        ReadV2Subtitles(stream);
                        break;
                    case 3:
                        ReadV3Subtitles(stream);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Subtitle format version not recognised: {0}", SubtitleVersion));
                }
            }
        }

        public string ReadSubtitle(Stream stream, Language language, long subtitleOffset, LocalizedVoiceSubtitleHeader localizedVoiceSubtitleHeader)
        {
            if (localizedVoiceSubtitleHeader.Length == 0)
            {
                return "";
            }

            long offset = subtitleOffset + localizedVoiceSubtitleHeader.Offset;
            stream.Seek(offset, SeekOrigin.Begin);
            byte[] subtitleData = new byte[localizedVoiceSubtitleHeader.Length];
            stream.Read(subtitleData, 0, (int)localizedVoiceSubtitleHeader.Length);

            var map = LanguageUtility.GetDecodeCharMap(Instance, language);

            StringBuilder subtitleBuilder = new StringBuilder();
            for (int pos = 0; pos < subtitleData.Length; pos += 2)
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
            return subtitle;
        }

        public void ReadV2Subtitles(Stream stream)
        {
            AudioMetadataSubtitleHeaderV2 SubtitleHeader = stream.ReadStruct<AudioMetadataSubtitleHeaderV2>();

            long subtitleOffset = stream.Position;

            for (int i = 0; i < SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders.Length; i++)
            {
                LocalizedVoiceSubtitleHeader localizedVoiceSubtitleHeader = SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i];
                Language language = (Language)i;

                MaleSubtitles.Add(language, ReadSubtitle(stream, language, subtitleOffset, localizedVoiceSubtitleHeader));
            }

            for (int i = 0; i < SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders.Length; i++)
            {
                LocalizedVoiceSubtitleHeader localizedVoiceSubtitleHeader = SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i];
                Language language = (Language)i;

                FemaleSubtitles.Add(language, ReadSubtitle(stream, language, subtitleOffset, localizedVoiceSubtitleHeader));
            }
        }

        public void ReadV3Subtitles(Stream stream)
        {
            AudioMetadataSubtitleHeaderV3 SubtitleHeader = stream.ReadStruct<AudioMetadataSubtitleHeaderV3>();

            long subtitleOffset = stream.Position;

            for (int i = 0; i < SubtitleHeader.LocalizedVoiceSubtitleHeaders.Length; i++)
            {
                LocalizedVoiceSubtitleHeader localizedVoiceSubtitleHeader = SubtitleHeader.LocalizedVoiceSubtitleHeaders[i];
                Language language = (Language)i;

                Subtitles.Add(language, ReadSubtitle(stream, language, subtitleOffset, localizedVoiceSubtitleHeader));
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

            if (Subtitles.Count != 0 || MaleSubtitles.Count != 0 || FemaleSubtitles.Count != 0)
            {
                switch (SubtitleVersion)
                {
                    case 2:
                        WriteV2Subtitles(stream);
                        break;
                    case 3:
                        WriteV3Subtitles(stream);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                Header.SubtitleSize = 0;
            }

            stream.Seek(0, SeekOrigin.Begin);
            stream.WriteStruct(Header);

            stream.SetLength(stream.Length.Align(2048));
        }

        private void WriteV2Subtitles(Stream stream)
        {
            uint startOfSubtitles = (uint)(0x24 + Header.LipsyncSize + 0xE4);

            uint nextSubtitleOffset = 0;

            AudioMetadataSubtitleHeaderV2 SubtitleHeader = new AudioMetadataSubtitleHeaderV2();
            SubtitleHeader.Version = 2;
            SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders = new LocalizedVoiceSubtitleHeader[14];
            SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders = new LocalizedVoiceSubtitleHeader[14];
            for (int i = 0; i < SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders.Length; i++)
            {
                Language language = (Language)i;

                string subtitle = MaleSubtitles[language];
                if (subtitle == "")
                {
                    SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i].Offset = nextSubtitleOffset;
                    SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i].Length = 0;
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
                    SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i].Offset = nextSubtitleOffset;
                    SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i].Length = (uint)subtitleData.Length;
                    stream.Seek(startOfSubtitles + nextSubtitleOffset, SeekOrigin.Begin);
                    stream.Write(subtitleData, 0, subtitleData.Length);

                    nextSubtitleOffset += (uint)subtitleData.Length;
                }
            }

            for (int i = 0; i < SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders.Length; i++)
            {
                Language language = (Language)i;

                if (FemaleSubtitles[language] == MaleSubtitles[language])
                {
                    SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i].Offset = SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i].Offset;
                    SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i].Length = SubtitleHeader.MaleLocalizedVoiceSubtitleHeaders[i].Length;

                    continue;
                }

                string subtitle = FemaleSubtitles[language];

                if (subtitle == "")
                {
                    SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i].Offset = nextSubtitleOffset;
                    SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i].Length = 0;
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
                    SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i].Offset = nextSubtitleOffset;
                    SubtitleHeader.FemaleLocalizedVoiceSubtitleHeaders[i].Length = (uint)subtitleData.Length;
                    stream.Seek(startOfSubtitles + nextSubtitleOffset, SeekOrigin.Begin);
                    stream.Write(subtitleData, 0, subtitleData.Length);

                    nextSubtitleOffset += (uint)subtitleData.Length;
                }
            }

            Header.SubtitleSize = nextSubtitleOffset + 0xE4;
            stream.Seek(0x24 + Header.SubtitleOffset, SeekOrigin.Begin);
            stream.WriteStruct(SubtitleHeader);
        }

        private void WriteV3Subtitles(Stream stream)
        {
            uint startOfSubtitles = (uint)(0x24 + Header.LipsyncSize + 0x74);

            uint nextSubtitleOffset = 0;

            AudioMetadataSubtitleHeaderV3 SubtitleHeader = new AudioMetadataSubtitleHeaderV3();
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
    }
}
