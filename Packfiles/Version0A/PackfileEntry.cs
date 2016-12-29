using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace ThomasJepp.SaintsRow.Packfiles.Version0A
{
    public class PackfileEntry : IPackfileEntry
    {
        private Packfile Packfile;
        public PackfileEntryFileData Data;
        private string Filename;

        public string Name
        {
            get { return Filename; }
        }

        public int Size
        {
            get { return (int)Data.Size; }
        }

        public bool HasStream
        {
            get { return !IsNew && Packfile.DataStream != null; }
        }

        public bool IsNew { get; set; }

        public Stream GetStream()
        {
            if (!this.HasStream)
            {
                return null;
            }

            byte[] data = new byte[Data.Size];
            long offset = Packfile.DataOffset + Data.Start;
            Packfile.DataStream.Seek(offset, SeekOrigin.Begin);
            if (Data.Flags.HasFlag(PackfileEntryFlags.Compressed))
            {
                byte[] compressedData = new byte[Data.CompressedSize];
                Packfile.DataStream.Read(compressedData, 0, (int)Data.CompressedSize);
                using (MemoryStream tempStream = new MemoryStream(compressedData))
                {
                    using (Stream s = new ZlibStream(tempStream, CompressionMode.Decompress, true))
                    {
                        s.Read(data, 0, (int)Data.Size);
                    }
                }
            }
            else
            {
                Packfile.DataStream.Read(data, 0, (int)Data.Size);
            }

            MemoryStream ms = new MemoryStream(data);
            return ms;
        }

        public PackfileEntry(Packfile packfile, PackfileEntryFileData data, string filename) : this(packfile, data, filename, false)
        {
        }
            public PackfileEntry(Packfile packfile, PackfileEntryFileData data, string filename, bool isNew)
        {
            Packfile = packfile;
            Data = data;
            Filename = filename;
            IsNew = isNew;
        }
    }
}
