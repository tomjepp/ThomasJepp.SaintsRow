using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

using ThomasJepp.SaintsRow.Stream2;

namespace ThomasJepp.SaintsRow.Packfiles.Version0A
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
            List<PackfileEntryFileData> entryFileData = new List<PackfileEntryFileData>();
            for (int i = 0; i < FileData.NumFiles; i++)
            {
                PackfileEntryFileData data = stream.ReadStruct<PackfileEntryFileData>();

                if (IsCondensed && IsCompressed)
                {
                    data.Flags = 0;
                    data.Start = runningPosition;
                    runningPosition += data.Size;
                }
                else if (IsCondensed)
                {
                    data.Start = runningPosition;
                    runningPosition += data.Size.Align(16);
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
                offset = offset.Align(2);
                offset += entry.Name.Length;
                offset += 2;
                offset = offset.Align(2);
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
            ZlibStream dataStream = null;
            if (IsCompressed && IsCondensed)
            {
                dataStream = new ZlibStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression, true);
                dataStream.FlushMode = FlushType.Sync;
            }

            long compressedStart = 0;

            for (int i = 0; i < Files.Count; i++)
            {
                IPackfileEntry entry = Files[i];
                Stream fs = m_Streams[entry.Name];

                bool isLast = (i == (Files.Count - 1));

                PackfileEntry pfE = (PackfileEntry)entry;
                var data = pfE.Data;
                data.Start = (uint)fileStart;
                data.Size = (uint)fs.Length;
                data.Alignment = (IsCondensed) ? (ushort)16 : (ushort)1;
                
                if (this.IsCompressed)
                    data.Flags = PackfileEntryFlags.Compressed;

                if (IsCompressed && IsCondensed)
                {
                    fs.CopyTo(dataStream);
                    dataStream.Flush();
                    long afterData = dataStream.TotalOut;
                    data.CompressedSize = (uint)(afterData - compressedStart);
                    compressedStart = afterData;

                    if (IsStr2)
                    {
                        fileStart += data.Size.Align(16);
                        uncompressedSize += data.Size;
                        compressedSize += data.CompressedSize;
                    }
                    else
                    {
                        fileStart += data.Size.Align(16);
                        uint toSkip = data.Size.Align(16) - data.Size;
                        uncompressedSize += data.Size.Align(16);
                        stream.Seek(toSkip, SeekOrigin.Current);
                        compressedSize += data.CompressedSize;
                    }
                }
                else if (IsCondensed)
                {
                    fs.CopyTo(stream);
                    data.CompressedSize = 0xFFFFFFFF;

                    fileStart += data.Size.Align(16);

                    if (isLast)
                        uncompressedSize += data.Size;
                    else
                    {
                        uint toSkip = data.Size.Align(16) - data.Size;
                        uncompressedSize += data.Size.Align(16);
                        stream.Seek(toSkip, SeekOrigin.Current);
                    }

                }
                else if (IsCompressed)
                {
                    long beforeData = stream.Position;
                    using (dataStream = new ZlibStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression, true))
                    {
                        dataStream.FlushMode = FlushType.Sync;
                        fs.CopyTo(dataStream);
                        dataStream.Flush();
                    }
                    long afterData = stream.Position;
                    data.CompressedSize = (uint)(afterData - beforeData);
                    fileStart += data.CompressedSize;
                    uncompressedSize += data.Size;
                    compressedSize += data.CompressedSize;
                }
                else
                {
                    fs.CopyTo(stream);
                    data.CompressedSize = 0xFFFFFFFF;
                    fileStart += data.Size;
                    uncompressedSize += data.Size;
                }

                fs.Close();

                pfE.Data = data;
            }

            if (IsCompressed && IsCondensed)
            {
                dataStream.Close();
            }

            // Output file names
            stream.Seek(CalculateEntryNamesOffset(), SeekOrigin.Begin);
            long startOffset = CalculateEntryNamesOffset();
            foreach (IPackfileEntry entry in Files)
            {
                PackfileEntry pfE = (PackfileEntry)entry;
                stream.Align(2);
                pfE.Data.FilenameOffset = (UInt32)(stream.Position - startOffset);
                stream.WriteAsciiNullTerminatedString(entry.Name);
                stream.Seek(1, SeekOrigin.Current);
                stream.Align(2);
            }
            long nameSize = stream.Position - CalculateEntryNamesOffset();

            // Output file info
            stream.Seek(GetEntryDataOffset(), SeekOrigin.Begin);
            foreach (IPackfileEntry entry in Files)
            {
                PackfileEntry pfE = (PackfileEntry)entry;
                stream.WriteStruct(pfE.Data);
            }
            long dirSize = stream.Position - GetEntryDataOffset();

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
            if (IsCompressed)
                FileData.CompressedDataSize = (uint)compressedSize;
            else
                FileData.CompressedDataSize = 0xFFFFFFFF;
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


        public void Update(Container container)
        {
            Dictionary<string, int> fileLookup = new Dictionary<string,int>();
            for (int i = 0; i < m_Files.Count; i++)
            {
                fileLookup.Add(m_Files[i].Name, i);
            }

            container.PackfileBaseOffset = (uint)CalculateDataStartOffset();
            container.TotalCompressedPackfileReadSize = (int)FileData.CompressedDataSize;
            foreach (Primitive primitive in container.Primitives)
            {
                if (!fileLookup.ContainsKey(primitive.Name))
                {
                    continue;
                }

                IPackfileEntry iEntry = m_Files[fileLookup[primitive.Name]];
                PackfileEntry entry = (PackfileEntry)iEntry;
                primitive.Data.CPUSize = entry.Data.Size;

                string gpuFile = "";
                string extension = Path.GetExtension(primitive.Name);
                string gpuExtension = "";
                switch (extension)
                {
                    default:
                        {
                            if (extension.StartsWith(".c"))
                                gpuExtension = ".g" + extension.Remove(0, 2);
                            break;
                        }
                }
                gpuFile = Path.ChangeExtension(primitive.Name, gpuExtension);

                if (fileLookup.ContainsKey(gpuFile))
                {
                    IPackfileEntry iGpuEntry = m_Files[fileLookup[gpuFile]];
                    PackfileEntry gpuEntry = (PackfileEntry)iGpuEntry;
                    primitive.Data.GPUSize = (uint)gpuEntry.Size;
                }
            }

            for (int i = 0; i < container.PrimitiveSizes.Count; i++)
            {
                var sizes = container.PrimitiveSizes[i];
                var primitiveData = container.Primitives[i];
                sizes.CPUSize = primitiveData.Data.CPUSize;
                sizes.GPUSize = primitiveData.Data.GPUSize;
                container.PrimitiveSizes[i] = sizes;
            }
        }
    }
}
