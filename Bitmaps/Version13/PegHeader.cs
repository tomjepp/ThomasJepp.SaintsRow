using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Bitmaps.Version13
{
    [StructLayout(LayoutKind.Explicit, Size = 0x18, CharSet = CharSet.Ansi)]
    public struct PegHeader
    {
        [FieldOffset(0x00)]
        public int Signature;

        [FieldOffset(0x04)]
        public short Version;

        [FieldOffset(0x06)]
        public short Platform;

        [FieldOffset(0x08)]
        public int DirBlockSize;

        [FieldOffset(0x0C)]
        public int DataBlockSize;

        [FieldOffset(0x10)]
        public short NumBitmaps;

        [FieldOffset(0x12)]
        public short Flags;

        [FieldOffset(0x14)]
        public short TotalEntries;

        [FieldOffset(0x16)]
        public short AlignValue;
    }
}
