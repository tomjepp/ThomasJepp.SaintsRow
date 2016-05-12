using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Bitmaps.Version13
{
    public class PegFile
    {
        public PegHeader Header;
        public List<PegEntry> Entries;

        public PegFile(Stream s)
        {
            Entries = new List<PegEntry>();

            Header = s.ReadStruct<PegHeader>();
            for (int i = 0; i < Header.TotalEntries; i++)
            {
                PegEntry entry = new PegEntry();
                entry.Data = s.ReadStruct<PegEntryData>();
                Entries.Add(entry);
            }

            for (int i = 0; i < Header.TotalEntries; i++)
            {
                PegEntry entry = Entries[i];
                string filename = s.ReadAsciiNullTerminatedString();
                entry.Filename = filename;
            }
        }

        public void Save(Stream s)
        {
            using (MemoryStream dirBlockStream = new MemoryStream())
            {
                foreach (PegEntry entry in Entries)
                {
                    dirBlockStream.WriteAsciiNullTerminatedString(entry.Filename);
                }

                // Header size + entry size + string size
                Header.DirBlockSize = (int)(0x18 + (0x48 * Entries.Count) + dirBlockStream.Length);

                s.WriteStruct(Header);

                foreach (PegEntry entry in Entries)
                {
                    s.WriteStruct(entry.Data);
                }

                dirBlockStream.Seek(0, SeekOrigin.Begin);
                dirBlockStream.CopyTo(s);
            }
        }
    }
}
