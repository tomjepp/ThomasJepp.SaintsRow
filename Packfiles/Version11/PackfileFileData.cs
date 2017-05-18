using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version11
{
    [StructLayout(LayoutKind.Explicit, Size = 0x78)]
    public struct PackfileFileData
    {
        [FieldOffset(0x00)]
        public UInt32 Descriptor;

        [FieldOffset(0x04)]
        public UInt32 Version;

        [FieldOffset(0x08)]
        public UInt32 HeaderChecksum;

        [FieldOffset(0x0C)]
        public PackfileFlags Flags;

        [FieldOffset(0x10)]
        public UInt32 NumFiles;

        [FieldOffset(0x14)]
        public UInt32 NumPaths;

        [FieldOffset(0x18)]
        public UInt32 DirSize;

        [FieldOffset(0x1C)]
        public UInt32 FilenameSize;

        [FieldOffset(0x20)]
        public UInt64 FileSize;

        [FieldOffset(0x28)]
        public UInt64 DataSize;

        [FieldOffset(0x30)]
        public UInt64 CompressedDataSize;

        [FieldOffset(0x38)]
        public UInt64 PackfileTimestamp;

        [FieldOffset(0x40)]
        public UInt64 DataOffset;
    }
}
