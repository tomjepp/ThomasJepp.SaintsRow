using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version06
{
    /*
     * struct v_packfile_entry {
     * char                        *filename;         // filename
     * 
     * uint32                      sector;            // sector offset RELATIVE to the start of the packfile
     * uint32                      start;             // offset from start of v_packfile::data (if data is valid) 
     * 
     * uint32                      size;              // file size
     * uint32                      compressed_size;   // compressed file size
     * struct v_packfile           *parent;           // my parent
     * };
     */
    [StructLayout(LayoutKind.Explicit, Size = 0x18)]
    public struct PackfileEntryFileData
    {
        [FieldOffset(0x00)]
        public UInt32 FilenameOffset;

        [FieldOffset(0x04)]
        public UInt32 Sector;

        [FieldOffset(0x08)]
        public UInt32 Start;

        [FieldOffset(0x0C)]
        public UInt32 Size;

        [FieldOffset(0x10)]
        public UInt32 CompressedSize;

        [FieldOffset(0x14)]
        public UInt32 Parent;
    }
}
