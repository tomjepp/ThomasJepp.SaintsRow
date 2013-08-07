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

            List<PackfileEntryFileData> entryFileData = new List<PackfileEntryFileData>();
            for (int i = 0; i < FileData.NumFiles; i++)
            {
                PackfileEntryFileData data = stream.ReadStruct<PackfileEntryFileData>();
                if (FileData.Flags.HasFlag(PackfileFlags.Condensed) && FileData.Flags.HasFlag(PackfileFlags.Compressed))
                    data.Flags = 0;
                  
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

            if (FileData.Flags.HasFlag(PackfileFlags.Condensed) && FileData.Flags.HasFlag(PackfileFlags.Compressed))
            {
                DataOffset = 0;
                using (Stream s = new DeflateStream(stream, CompressionMode.Decompress, true))
                {
                    byte[] uncompressedData = new byte[FileData.DataSize];
                    s.Read(uncompressedData, 0, (int)FileData.DataSize);
                    DataStream = new MemoryStream(uncompressedData);
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
    }
}
