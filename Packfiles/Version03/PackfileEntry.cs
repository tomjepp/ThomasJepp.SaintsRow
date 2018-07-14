using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace ThomasJepp.SaintsRow.Packfiles.Version03
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
            byte[] data = new byte[Data.Size];

            Packfile.DataStream.Seek(Packfile.DataOffset + Data.Start, SeekOrigin.Begin);
            Packfile.DataStream.Read(data, 0, (int)Data.Size);

            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
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
