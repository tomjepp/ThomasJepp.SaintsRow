using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Strings
{
    [StructLayout(LayoutKind.Explicit, Size = 0x08)]
    public struct StringBucket
    {
        [FieldOffset(0x00)]
        public UInt32 StringCount;

        [FieldOffset(0x04)]
        public UInt32 StringOffset;
    }
}
