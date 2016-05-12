using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    public class StreamingSoundbank : IDisposable
    {
        public SoundbankHeader Header = new SoundbankHeader();
        private List<SoundbankEntry> m_Files = new List<SoundbankEntry>();
        public Stream DataStream;

        private List<Stream> m_AudioStreams = new List<Stream>();
        private List<Stream> m_MetadataStreams = new List<Stream>();

        public StreamingSoundbank(Stream s)
        {
            DataStream = new MemoryStream();
            s.CopyTo(DataStream);

            DataStream.Seek(0, SeekOrigin.Begin);

            Header = DataStream.ReadStruct<SoundbankHeader>();

            if (Header.Signature != 0x42535756)
                throw new InvalidDataException("File is not a streaming soundbank.");

            for (int i = 0; i < Header.NumFiles; i++)
            {
                var fileInfo = DataStream.ReadStruct<SoundbankEntryInfo>();
                var entry = new SoundbankEntry(this, fileInfo);
                Files.Add(entry);
            }
        }

        public StreamingSoundbank()
        {
            Header = new SoundbankHeader();
            Header.Signature = 0x42535756;
            Header.Platform = 0x20204350;
            Header.Timestamp = 0;
            Header.Version = 2;
            Header.CruncherVersion = 1;

        }

        public List<SoundbankEntry> Files
        {
            get { return m_Files; }
        }

        public SoundbankEntry this[int i]
        {
            get { return m_Files[i]; }
        }

        public void AddFile(uint id, Stream audioStream)
        {
            AddFile(id, audioStream, null);
        }

        public void AddFile(uint id, Stream audioStream, Stream metadataStream)
        {
            SoundbankEntry entry = new SoundbankEntry(this);
            entry.Info.FileId = id;
            if (metadataStream != null)
                entry.Info.MetadataLength = (uint)metadataStream.Length;
            else
                entry.Info.MetadataLength = 0;
            entry.Info.AudioLength = (uint)audioStream.Length;
            m_AudioStreams.Add(audioStream);
            m_MetadataStreams.Add(metadataStream);
            Files.Add(entry);
        }

        public void Save(Stream bnkStream, Stream mbnkStream)
        {
            Header.NumFiles = (uint)Files.Count;
            Header.HeaderSize = (uint)(Marshal.SizeOf(typeof(SoundbankHeader)) + (Marshal.SizeOf(typeof(SoundbankEntryInfo)) * Header.NumFiles));
            bnkStream.WriteStruct(Header);
            mbnkStream.WriteStruct(Header);
            uint nextOffset = Header.HeaderSize;
            nextOffset = nextOffset.Align(0x800);
            foreach (var entry in Files)
            {
                entry.Info.Offset = nextOffset;
                nextOffset += entry.Info.MetadataLength + entry.Info.AudioLength;
                nextOffset = nextOffset.Align(0x800);
                bnkStream.WriteStruct(entry.Info);
                mbnkStream.WriteStruct(entry.Info);
            }

            int count = 0;
            foreach (var entry in Files)
            {
                bnkStream.Seek(entry.Info.Offset, SeekOrigin.Begin);
                Stream metadataStream = m_MetadataStreams[count];
                if (metadataStream != null)
                {
                    metadataStream.Seek(0, SeekOrigin.Begin);
                    metadataStream.CopyTo(bnkStream);
                }
                Stream audioStream = m_AudioStreams[count];
                audioStream.CopyTo(bnkStream);
                count++;
            }

        }

        public void Dispose()
        {
            if (DataStream != null)
            {
                DataStream.Dispose();
                DataStream = null;
            }
        }
    }
}
