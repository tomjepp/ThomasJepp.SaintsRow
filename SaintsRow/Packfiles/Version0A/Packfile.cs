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

        public long DataOffset = 0;
        public Stream DataStream;

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
        }

        public bool IsCondensed
        {
            get { return FileData.Flags.HasFlag(PackfileFlags.Condensed); }
        }
    }
}
