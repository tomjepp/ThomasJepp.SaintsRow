using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Saves.SaintsRowIVMod.Sections;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod
{
    public class SaveFile
    {
        public SaveGameMainHeader MainHeader;
        private Dictionary<SectionId, Section> Sections = new Dictionary<SectionId, Section>();

        public PlayerSection Player;

        public SaveFile(Stream s)
        {
            MainHeader = s.ReadStruct<SaveGameMainHeader>();

            while (s.Position < s.Length)
            {
                long sectionStart = s.Position;
                Section section = new Section(s);
                if (section.Size == 0 && section.Version == 0)
                    break;
                
                Console.WriteLine("Got {0} ({4:X2}) at {3:X4}. Version {1:X2}, {2:X4} bytes.", section.SectionId, section.Version, section.Size, sectionStart, (uint)section.SectionId);
                Sections.Add(section.SectionId, section);
            }

            Player = new PlayerSection(Sections[SectionId.GSSI_PLAYER]);
        }

        public void Save(Stream s)
        {
            byte[] sectionData = null;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (Section section in Sections.Values)
                {
                    section.Save(ms);
                }
                sectionData = ms.ToArray();
            }
            MainHeader.Checksum = Hashes.CrcVolition(sectionData);
            s.WriteStruct(MainHeader);
            s.Write(sectionData, 0, sectionData.Length);
        }
    }
}
