using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version0A
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
    [StructLayout(LayoutKind.Explicit, Size = 0x28)]
    public struct PackfileFileData
    {
        [FieldOffset(0x00)]
        public UInt32 Descriptor;

        [FieldOffset(0x04)]
        public UInt32 Version;

        [FieldOffset(0x08)]
        public UInt32 HeaderChecksum;

        [FieldOffset(0x0C)]
        public UInt32 FileSize;
        
        [FieldOffset(0x10)]
        public PackfileFlags Flags;

        [FieldOffset(0x14)]
        public UInt32 NumFiles;

        [FieldOffset(0x18)]
        public UInt32 DirSize;

        [FieldOffset(0x1C)]
        public UInt32 FilenameSize;

        [FieldOffset(0x20)]
        public UInt32 DataSize;

        [FieldOffset(0x24)]
        public UInt32 CompressedDataSize;
    }
}
