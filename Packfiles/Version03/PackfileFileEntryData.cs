using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version03
{
    [StructLayout(LayoutKind.Explicit, Size = 0x1C)]
    public struct PackfileEntryFileData
    {
        [FieldOffset(0x00)] // length = 0x4
        public uint Name;

        [FieldOffset(0x04)] // length = 0x4
        public uint Sector;

        [FieldOffset(0x08)] // length = 0x4
        public uint Start;

        [FieldOffset(0x0C)] // length = 0x4
        public uint Hash;

        [FieldOffset(0x10)] // length = 0x4
        public uint Size;

        [FieldOffset(0x14)] // length = 0x4
        public uint CompressedSize;

        [FieldOffset(0x18)] // length = 0x4
        public uint Parent;
    }
}
