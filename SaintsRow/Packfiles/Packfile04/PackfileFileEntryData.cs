using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version04
{
    [StructLayout(LayoutKind.Explicit, Size = 0x1C)]
    public struct PackfileEntryFileData
    {
        [FieldOffset(0x00)]
        public UInt32 FilenameOffset;

        [FieldOffset(0x04)]
        public UInt32 ExtensionOffset;

        [FieldOffset(0x08)]
        public UInt32 Unknown08;

        [FieldOffset(0x0C)]
        public UInt32 Start;

        [FieldOffset(0x10)]
        public UInt32 Size;

        [FieldOffset(0x14)]
        public UInt32 CompressedSize;

        [FieldOffset(0x18)]
        public UInt32 Unknown18;
    }
}
