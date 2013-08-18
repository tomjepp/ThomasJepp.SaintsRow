using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version0A
{
    /*
     * UserDefinedType:     v_packfile_entry_file_data::<unnamed-type-m_filename>
     * Data           :   this+0x8, Member, Type: unsigned int, start
     * Data           :   this+0xC, Member, Type: unsigned int, size
     * Data           :   this+0x10, Member, Type: unsigned int, compressed_size
     * Data           :   this+0x14, Member, Type: unsigned short, flags
     * Data           :   this+0x16, Member, Type: unsigned short, alignment
     */
    [StructLayout(LayoutKind.Explicit, Size = 0x18)]
    public struct PackfileEntryFileData
    {
        [FieldOffset(0x00)]
        public UInt32 FilenameOffset;

        [FieldOffset(0x08)]
        public UInt32 Start;

        [FieldOffset(0x0C)]
        public UInt32 Size;

        [FieldOffset(0x10)]
        public UInt32 CompressedSize;

        [FieldOffset(0x14)]
        public PackfileEntryFlags Flags;

        [FieldOffset(0x16)]
        public UInt16 Alignment;
    }
}
