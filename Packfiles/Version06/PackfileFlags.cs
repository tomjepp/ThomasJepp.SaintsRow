using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Packfiles.Version06
{
    public enum PackfileFlags : uint
    {
        Compressed = 0x01,
        Condensed = 0x02
    }
}
