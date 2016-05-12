using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections
{
    public class STMGSection : ISoundbankSection
    {
        public SectionId SectionId { get { return Wwise.SectionId.STMG; } }

        public byte[] Data { get; set; }

        public STMGSection(byte[] data)
        {
            Data = data;
        }
    }
}
