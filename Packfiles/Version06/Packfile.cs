using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

using ThomasJepp.SaintsRow.AssetAssembler;

namespace ThomasJepp.SaintsRow.Packfiles.Version06
{
    public class Packfile : IPackfile
    {
        private List<IPackfileEntry> m_Files;
        private PackfileFileData FileData;

        private Dictionary<string, Stream> m_Streams;

        public long DataOffset = 0;
        public Stream DataStream;
        public bool IsStr2;

        public Packfile(bool isStr2)
        {
            IsStr2 = isStr2;
            m_Files = new List<IPackfileEntry>();
            m_Streams = new Dictionary<string, Stream>();
        }

        public Packfile(Stream stream, bool isStr2)
        {
            IsStr2 = isStr2;
            stream.Seek(0, SeekOrigin.Begin);
            FileData = stream.ReadStruct<PackfileFileData>();

            m_Files = new List<IPackfileEntry>();

            uint runningPosition = 0;
            stream.Seek(GetEntryDataOffset(), SeekOrigin.Begin);
            List<PackfileEntryFileData> entryFileData = new List<PackfileEntryFileData>();
            for (int i = 0; i < FileData.NumFiles; i++)
            {
                PackfileEntryFileData data = stream.ReadStruct<PackfileEntryFileData>();

                if (IsCondensed && IsCompressed)
                {
                    data.Start = runningPosition;
                    runningPosition += data.Size;
                }
                else if (IsCondensed)
                {
                    data.Start = runningPosition;
                    runningPosition += data.Size.Align(16);
                }
                else if (IsCompressed)
                {
                    data.Start = runningPosition;
                    runningPosition += data.CompressedSize.Align(2048);
                }
                entryFileData.Add(data);
            }

            for (int i = 0; i < FileData.NumFiles; i++)
            {
                PackfileEntryFileData data = entryFileData[i];

                stream.Seek(CalculateEntryNamesOffset() + data.FilenameOffset, SeekOrigin.Begin);
                string filename = stream.ReadAsciiNullTerminatedString();
                m_Files.Add(new PackfileEntry(this, data, filename));
            }

            if (IsCondensed && IsCompressed)
            {
                stream.Seek(CalculateDataStartOffset(), SeekOrigin.Begin);

                DataOffset = 0;
                byte[] compressedData = new byte[FileData.CompressedDataSize];
                stream.Read(compressedData, 0, (int)FileData.CompressedDataSize);
                using (MemoryStream tempStream = new MemoryStream(compressedData))
                {
                    using (Stream s = new ZlibStream(tempStream, CompressionMode.Decompress, true))
                    {
                        byte[] uncompressedData = new byte[FileData.DataSize];
                        s.Read(uncompressedData, 0, (int)FileData.DataSize);
                        DataStream = new MemoryStream(uncompressedData);
                    }
                }
            }
            else
            {
                DataStream = stream;
                DataOffset = CalculateDataStartOffset();
            }
        }

        public void Dispose()
        {
            if (DataOffset == 0)
            {
                if (DataStream != null)
                {
                    DataStream.Dispose();
                }
            }
        }

        public List<IPackfileEntry> Files
        {
            get { return m_Files; }
        }

        public IPackfileEntry this[int i]
        {
            get { return m_Files[i]; }
        }

        public int Version
        {
            get
            {
                return 0x06;
            }
        }


        public bool IsCompressed
        {
            get { return FileData.Flags.HasFlag(PackfileFlags.Compressed); }
            set { if (value) { FileData.Flags |= PackfileFlags.Compressed; } else { FileData.Flags &= ~PackfileFlags.Compressed; } }
        }

        public bool IsCondensed
        {
            get { return FileData.Flags.HasFlag(PackfileFlags.Condensed); }
            set { if (value) { FileData.Flags |= PackfileFlags.Condensed; } else { FileData.Flags &= ~PackfileFlags.Condensed; } }
        }


        public void AddFile(Stream stream, string filename)
        {
            Files.Add(new PackfileEntry(this, new PackfileEntryFileData(), filename));
            m_Streams.Add(filename, stream);
        }

        private long GetEntryDataOffset()
        {
            return (0x180).Align(2048);
        }

        private long CalculateEntryNamesOffset()
        {
            return (GetEntryDataOffset() + FileData.DirectorySize).Align(2048);
        }

        private long CalculateDataStartOffset()
        {
            long offset = (CalculateEntryNamesOffset() + FileData.FilenamesSize).Align(2048);

            return offset;
        }

        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }


        public void Update(IContainer container)
        {
            throw new NotImplementedException();
        }

        public bool ContainsFile(string filename)
        {
            foreach (PackfileEntry entry in Files)
            {
                if (entry.Name == filename)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveFile(string filename)
        {
            PackfileEntry entry = null;

            foreach (PackfileEntry e in Files)
            {
                if (e.Name == filename)
                {
                    entry = e;
                    break;
                }
            }

            if (entry == null)
                return;

            RemoveFile(entry);
        }

        public void RemoveFile(IPackfileEntry entry)
        {
            Files.Remove(entry);
            if (m_Streams != null && m_Streams.ContainsKey(entry.Name))
                m_Streams.Remove(entry.Name);
        }
    }
}
