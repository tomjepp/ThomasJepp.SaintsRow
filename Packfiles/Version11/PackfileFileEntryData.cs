using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version11
{
    [StructLayout(LayoutKind.Explicit, Size = 0x30)]
    public struct PackfileEntryFileData
    {
        [FieldOffset(0x00)]
        public UInt64 FilenameOffset;

        [FieldOffset(0x08)]
        public UInt64 Filepath;

        [FieldOffset(0x10)]
        public UInt64 Start;

        [FieldOffset(0x18)]
        public UInt64 Size;

        [FieldOffset(0x20)]
        public UInt64 CompressedSize;

        [FieldOffset(0x28)]
        public PackfileEntryFlags Flags;

        [FieldOffset(0x2A)]
        public UInt32 Alignment;
    }
}
