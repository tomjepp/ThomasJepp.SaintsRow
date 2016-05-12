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
            byte[] data = new byte[Data.Size];

            if (Packfile.IsCompressed)
            {
                // No SR2 packages for PC are compressed?
                throw new NotImplementedException();
            }
            else
            {
                Packfile.DataStream.Seek(Packfile.CalculateDataStartOffset() + Data.Start, SeekOrigin.Begin);
                Packfile.DataStream.Read(data, 0, (int)Data.Size);
            }

            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public PackfileEntry(Packfile packfile, PackfileEntryFileData data, string filename)
        {
            Packfile = packfile;
            Data = data;
            Filename = filename;
        }
    }
}
