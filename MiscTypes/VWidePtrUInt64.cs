using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.MiscTypes
{
    [StructLayout(LayoutKind.Explicit, Size = 0x08, CharSet = CharSet.Ansi)]
    public struct VWidePtrUInt64
    {
        [FieldOffset(0x00)]
        public UInt64 Value;
    }
}
