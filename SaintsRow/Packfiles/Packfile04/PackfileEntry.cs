using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace ThomasJepp.SaintsRow.Packfiles.Version04
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

        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public PackfileEntry(Packfile packfile, PackfileEntryFileData data, string filename)
        {
            Packfile = packfile;
            Data = data;
            Filename = filename;
        }
    }
}
