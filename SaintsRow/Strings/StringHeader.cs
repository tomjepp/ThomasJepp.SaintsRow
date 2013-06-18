using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Strings
{
    [StructLayout(LayoutKind.Explicit, Size=0x0C)]
    public struct StringHeader
    {
        [FieldOffset(0x00)]
        public UInt32 ID;

        [FieldOffset(0x04)]
        public UInt16 Version;

        [FieldOffset(0x06)]
        public UInt16 BucketCount;

        [FieldOffset(0x08)]
        public UInt32 StringCount;
    }
}
