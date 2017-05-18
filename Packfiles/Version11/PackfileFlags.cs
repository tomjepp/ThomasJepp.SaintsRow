using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Packfiles.Version11
{
    public enum PackfileFlags : uint
    {
        Compressed = 0x01,
        Condensed = 0x02
    }

    public enum PackfileEntryFlags : uint
    {
        Compressed = 0x01
    }
}
