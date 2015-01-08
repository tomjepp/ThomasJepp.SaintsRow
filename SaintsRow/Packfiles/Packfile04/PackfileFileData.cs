using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version04
{
    /*
     * UserDefinedType: v_packfile_file_data
     * Data           :   this+0x0, Member, Type: unsigned int, descriptor
     * Data           :   this+0x4, Member, Type: unsigned int, version
     * Data           :   this+0x8, Member, Type: unsigned int, header_checksum
     * Data           :   this+0xC, Member, Type: unsigned int, file_size
     * Data           :   this+0x10, Member, Type: unsigned int, flags
     * Data           :   this+0x14, Member, Type: unsigned int, num_files
     * Data           :   this+0x18, Member, Type: unsigned int, dir_size
     * Data           :   this+0x1C, Member, Type: unsigned int, filename_size
     * Data           :   this+0x20, Member, Type: unsigned int, data_size
     * Data           :   this+0x24, Member, Type: unsigned int, compressed_data_size
     */
    [StructLayout(LayoutKind.Explicit, Size = 0x180)]
    public struct PackfileFileData
    {
        [FieldOffset(0x00)]
        public UInt32 Descriptor;

        [FieldOffset(0x04)]
        public UInt32 Version;

        [FieldOffset(0x14C)]
        public PackfileFlags Flags;

        [FieldOffset(0x154)]
        public UInt32 IndexCount;

        [FieldOffset(0x158)]
        public UInt32 PackageSize;

        [FieldOffset(0x15C)]
        public UInt32 IndexSize;

        [FieldOffset(0x160)]
        public UInt32 NamesSize;

        [FieldOffset(0x164)]
        public UInt32 ExtensionsSize;

        [FieldOffset(0x168)]
        public UInt32 UncompressedDataSize;

        [FieldOffset(0x16C)]
        public UInt32 CompressedDataSize;
    }
}
