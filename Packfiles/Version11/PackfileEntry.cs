using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using lz4;

namespace ThomasJepp.SaintsRow.Packfiles.Version11
{
    public class PackfileEntry : IPackfileEntry
    {
        private Packfile Packfile;
        public PackfileEntryFileData Data;
        private string Filename;
        private string FilePath;

        public string Path
        {
            get { return FilePath; }
        }

        public string FullPath
        {
            get { return System.IO.Path.Combine(FilePath, Name); }
        }

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

            long offset = (Packfile.DataOffset + (long)Data.Start);
            Packfile.DataStream.Seek(offset, SeekOrigin.Begin);

            if (Data.Flags.HasFlag(PackfileEntryFlags.Compressed))
            {
                MemoryStream uncompressedData;
                using (MemoryStream compressedStream = Packfile.DataStream.ReadMemoryStream(Data.CompressedSize))
                {
                    using (LZ4Stream s = LZ4Stream.CreateDecompressor(compressedStream, LZ4StreamMode.Read, true))
                    {
                        uncompressedData = s.ReadMemoryStream(Data.Size);
                    }
                }
                return uncompressedData;
            }
            else
            {
                return Packfile.DataStream.ReadMemoryStream(Data.Size);
            }
        }

        public PackfileEntry(Packfile packfile, PackfileEntryFileData data, string filename, string path) : this(packfile, data, filename, path, false)
        {
        }
        public PackfileEntry(Packfile packfile, PackfileEntryFileData data, string filename, string path, bool isNew)
        {
            Packfile = packfile;
            Data = data;
            Filename = filename;
            FilePath = path;
            IsNew = isNew;
        }
    }
}
