using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise
{
    public class WwiseSoundbank
    {
        public List<ISoundbankSection> Sections = new List<ISoundbankSection>();
        public WwiseSoundbank(Stream s)
        {
            while (s.Position < s.Length)
            {
                SectionId sectionId = (SectionId)s.ReadUInt32();
                uint length = s.ReadUInt32();
                byte[] data = new byte[length];
                s.Read(data, 0, (int)length);

                switch (sectionId)
                {
                    case SectionId.BKHD:
                        BKHDSection bkhd = new BKHDSection(data);
                        Sections.Add(bkhd);
                        break;
                    case SectionId.ENVS:
                        ENVSSection envs = new ENVSSection(data);
                        Sections.Add(envs);
                        break;
                    case SectionId.HIRC:
                        HIRCSection hirc = new HIRCSection(data);
                        Sections.Add(hirc);
                        break;
                    case SectionId.STMG:
                        STMGSection stmg = new STMGSection(data);
                        Sections.Add(stmg);
                        break;
                    default:
                        throw new NotImplementedException("Unknown section: " + sectionId.ToString());
                }
            }
        }
    }
}
