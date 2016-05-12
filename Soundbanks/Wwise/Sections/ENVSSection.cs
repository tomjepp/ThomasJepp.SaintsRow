using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections
{
    public class ENVSSection : ISoundbankSection
    {
        public SectionId SectionId { get { return Wwise.SectionId.ENVS; } }

        public byte[] Data { get; set; }

        public ENVSSection(byte[] data)
        {
            Data = data;
        }
    }
}
