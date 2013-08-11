using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace ThomasJepp.SaintsRow.Packfiles.Version0A
{
    public class Packfile : IPackfile
    {
        private List<IPackfileEntry> m_Files;
        private PackfileFileData FileData;

        private Dictionary<string, Stream> m_Streams;

        public long DataOffset = 0;
        public Stream DataStream;

        public Packfile()
        {
            m_Files = new List<IPackfileEntry>();
            m_Streams = new Dictionary<string, Stream>();
        }

        public Packfile(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            FileData = stream.ReadStruct<PackfileFileData>();

            m_Files = new List<IPackfileEntry>();

            uint runningPosition = 0;
            List<PackfileEntryFileData> entryFileData = new List<PackfileEntryFileData>();
            for (int i = 0; i < FileData.NumFiles; i++)
            {
                PackfileEntryFileData data = stream.ReadStruct<PackfileEntryFileData>();
                if (IsCondensed && IsCompressed)
                    data.Flags = 0;

                if (IsCondensed)
                {
                    data.Start = runningPosition;
                    runningPosition += data.Size;
                }

                entryFileData.Add(data);
            }

            for (int i = 0; i < FileData.NumFiles; i++)
            {
                stream.Align(2);
                string filename = stream.ReadAsciiNullTerminatedString();
                stream.Seek(1, SeekOrigin.Current);
                m_Files.Add(new PackfileEntry(this, entryFileData[i], filename));
                stream.Align(2);
            }

            if (IsCondensed && IsCompressed)
            {
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
                DataOffset = stream.Position;
            }
        }

        public void Dispose()
        {
            if (DataOffset == 0)
                DataStream.Dispose();
        }

        public List<IPackfileEntry> Files
        {
            get { return m_Files; }
        }

        public IPackfileEntry this[int i]
        {
            get { return m_Files[i]; }
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
            return 0x28;
        }

        private long CalculateEntryNamesOffset()
        {
            return 0x28 + (0x18 * Files.Count);
        }

        private long CalculateDataStartOffset()
        {
            long offset = CalculateEntryNamesOffset();

            foreach (IPackfileEntry entry in Files)
            {
                offset.Align(2);
                offset += entry.Name.Length;
                offset += 1;
                offset.Align(2);
            }

            return offset;
        }

        public void Save(Stream stream)
        {
            long dataStart = CalculateDataStartOffset();

            stream.Seek(dataStart, SeekOrigin.Begin);

            long fileStart = 0;

            long compressedSize = 0;
            long uncompressedSize = 0;

            // Output file data
            Stream dataStream = null;
            if (IsCompressed && IsCondensed)
            {
                dataStream = new ZlibStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression, true);
            }

            foreach (IPackfileEntry entry in Files)
            {
                Stream fs = m_Streams[entry.Name];

                PackfileEntry pfE = (PackfileEntry)entry;
                var data = pfE.Data;
                data.Start = (uint)fileStart;
                data.Size = (uint)fs.Length;
                data.Alignment = (IsCompressed && IsCondensed) ? (ushort)16 : (ushort)1;
                
                if (this.IsCompressed)
                    data.Flags = PackfileEntryFlags.Compressed;

                if (IsCompressed && IsCondensed)
                {
                    long beforeData = stream.Position;
                    fs.CopyTo(dataStream);
                    dataStream.Flush();
                    stream.Flush();
                    long afterData = stream.Position;
                    data.CompressedSize = (uint)(afterData - beforeData);
                    fileStart += data.CompressedSize;
                    fileStart.Align(16);
                    
                }
                else if (IsCompressed)
                {
                    using (dataStream = new ZlibStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression, true))
                    {
                        long beforeData = stream.Position;
                        fs.CopyTo(dataStream);
                        dataStream.Flush();
                        stream.Flush();
                        long afterData = stream.Position;
                        data.CompressedSize = (uint)(afterData - beforeData);
                        fileStart += data.CompressedSize;
                    }
                }
                else
                {
                    fs.CopyTo(dataStream);
                    data.CompressedSize = 0xFFFFFFFF;
                }

                uncompressedSize += data.Size;
                compressedSize += data.CompressedSize;

                fs.Close();

                pfE.Data = data;
            }

            if (IsCompressed && IsCondensed)
            {
                dataStream.Close();
            }

            // Output file info
            stream.Seek(GetEntryDataOffset(), SeekOrigin.Begin);
            foreach (IPackfileEntry entry in Files)
            {
                PackfileEntry pfE = (PackfileEntry)entry;
                var data = pfE.Data;
                stream.WriteStruct(data);
            }
            long dirSize = stream.Position - GetEntryDataOffset();

            // Output file names
            stream.Seek(CalculateEntryNamesOffset(), SeekOrigin.Begin);
            foreach (IPackfileEntry entry in Files)
            {
                stream.Align(2);
                stream.WriteAsciiNullTerminatedString(entry.Name);
                stream.Seek(1, SeekOrigin.Current);
                stream.Align(2);
            }
            long nameSize = stream.Position - CalculateEntryNamesOffset();

            // Output header
            stream.Seek(0, SeekOrigin.Begin);
            FileData.Descriptor = 0x51890ACE;
            FileData.Version = 0x0A;
            FileData.HeaderChecksum = 0;
            FileData.FileSize = (uint)stream.Length;
            FileData.NumFiles = (uint)Files.Count;
            FileData.DirSize = (uint)dirSize;
            FileData.FilenameSize = (uint)nameSize;
            FileData.DataSize = (uint)uncompressedSize;
            FileData.CompressedDataSize = (uint)compressedSize;
            stream.WriteStruct(FileData);

            uint checksum = 0;
            byte[] checksumBuffer = new byte[0x1C];

            stream.Seek(0x0C, SeekOrigin.Begin);
            stream.Read(checksumBuffer, 0, checksumBuffer.Length);
            checksum = Hashes.CrcVolition(checksumBuffer);

            stream.Seek(0x08, SeekOrigin.Begin);
            stream.WriteUInt32(checksum);

            stream.Flush();
        }
    }
}
