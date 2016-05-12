using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    public class SoundbankEntry
    {
        public StreamingSoundbank Soundbank { get; private set; }

        public SoundbankEntryInfo Info;

        public SoundbankEntry(StreamingSoundbank bank, SoundbankEntryInfo info)
        {
            Soundbank = bank;
            Info = info;
        }

        public SoundbankEntry(StreamingSoundbank bank)
        {
        }

        public Stream GetAudioStream()
        {
            Soundbank.DataStream.Seek(Info.Offset + Info.MetadataLength, SeekOrigin.Begin);
            byte[] audioData = new byte[Info.AudioLength];
            Soundbank.DataStream.Read(audioData, 0, audioData.Length);
            MemoryStream audioStream = new MemoryStream(audioData);

            return audioStream;
        }

        public Stream GetMetadataStream()
        {
            Soundbank.DataStream.Seek(Info.Offset, SeekOrigin.Begin);
            byte[] metadata = new byte[Info.MetadataLength];
            Soundbank.DataStream.Read(metadata, 0, metadata.Length);
            MemoryStream audioStream = new MemoryStream(metadata);

            return audioStream;
        }
    }
}
