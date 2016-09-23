using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Bitmaps.Version13
{
    public class PegEntry
    {
        public string Filename;
        public PegEntryData Data;

        public PegEntry()
        {
        }

        public byte[] GetData(Stream s)
        {
            s.Seek(Data.DataPtr.Value, SeekOrigin.Begin);
            long length = Data.FrameSize * Data.NumFrames;
            byte[] data = new byte[length];

            long read = 0;
            while (read < length)
            {
                read += s.Read(data, (int)read, (int)(length - read));
            }

            return data;
        }
    }
}
