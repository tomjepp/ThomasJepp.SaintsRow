using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise
{
    public interface ISoundbankSection
    {
        SectionId SectionId { get; }
        byte[] Data { get; set; }
    }
}
