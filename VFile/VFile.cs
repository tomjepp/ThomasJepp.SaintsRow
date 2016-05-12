using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.VFile
{
    public class VFile
    {
        private VFileHeader Header;
        public List<string> References;

        public VFile()
        {
            Header = new VFileHeader();
            References = new List<string>();
        }

        public VFile(Stream s)
        {
            References = new List<string>();

            Header = s.ReadStruct<VFileHeader>();
            long position = s.Position;
            s.Seek(position + Header.ReferenceDataStart, SeekOrigin.Begin);
            for (int i = 0; i < Header.ReferenceCount; i++)
            {
                string reference = s.ReadAsciiNullTerminatedString();
                References.Add(reference);
            }

            s.Seek(position + Header.ReferenceDataSize + 1, SeekOrigin.Begin);

            s.Align(16);
        }

        public void Save(Stream s)
        {
            using (MemoryStream referenceStream = new MemoryStream())
            {
                foreach (string reference in References)
                {
                    referenceStream.WriteAsciiNullTerminatedString(reference);
                }

                Header.ReferenceCount = (uint)References.Count;
                Header.ReferenceDataSize = (uint)referenceStream.Length;
                s.WriteStruct(Header);
                long position = s.Position;
                s.Seek(position + Header.ReferenceDataStart, SeekOrigin.Begin);
                referenceStream.Seek(0, SeekOrigin.Begin);
                referenceStream.CopyTo(s);
                s.Seek(position + Header.ReferenceDataSize + 1, SeekOrigin.Begin);

                s.Align(16);
            }
        }
    }
}
