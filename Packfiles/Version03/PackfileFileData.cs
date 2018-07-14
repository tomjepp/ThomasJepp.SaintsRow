using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version03
{
    [StructLayout(LayoutKind.Explicit, Size = 0x17C, CharSet=CharSet.Ansi)]
    public struct PackfileFileData // v_packfile
    {
        [FieldOffset(0x00)] // length = 0x4
        public uint Descriptor;

        [FieldOffset(0x04)] // length = 0x4
        public uint Version;

        //[FieldOffset(0x08)] // length = 0x64
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        //public string ShortName;

        //[FieldOffset(0x4C)] // length = 0x256
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        //public byte[] Pathname;

        [FieldOffset(0x14C)] // length = 0x4
        public PackfileFlags Flags;

        [FieldOffset(0x150)] // length = 0x4
        public uint Sector;

        [FieldOffset(0x154)] // length = 0x4
        public uint NumFiles;

        [FieldOffset(0x158)] // length = 0x4
        public uint FileSize;

        [FieldOffset(0x15C)] // length = 0x4
        public uint DirSize;

        [FieldOffset(0x160)] // length = 0x4
        public uint FilenameSize;

        [FieldOffset(0x164)] // length = 0x4
        public uint DataSize;

        [FieldOffset(0x168)] // length = 0x4
        public uint CompressedDataSize;

        [FieldOffset(0x16C)] // length = 0x4
        public uint Dir;

        [FieldOffset(0x170)] // length = 0x4
        public uint DirSerialized;

        [FieldOffset(0x174)] // length = 0x4
        public uint Filenames;

        [FieldOffset(0x178)] // length = 0x4
        public uint Data;

    }
}
