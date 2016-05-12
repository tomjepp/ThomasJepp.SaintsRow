using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;

namespace ThomasJepp.SaintsRow.VFile
{
    [StructLayout(LayoutKind.Explicit, Size = 0x20, CharSet = CharSet.Ansi)]
    public struct VFileHeader
    {
        [FieldOffset(0x00)]
        public UInt16 Signature;

        [FieldOffset(0x02)]
        public UInt16 Version;

        [FieldOffset(0x04)]
        public UInt32 ReferenceDataSize;

        [FieldOffset(0x08)]
        public UInt32 ReferenceDataStart;

        [FieldOffset(0x0C)]
        public UInt32 ReferenceCount;

        [FieldOffset(0x10)]
        public byte Initialized;
    }
}
