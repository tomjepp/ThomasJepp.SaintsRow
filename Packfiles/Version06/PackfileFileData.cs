using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Packfiles.Version06
{
    /*
     * struct v_packfile {
     * uint32            descriptor;        // packfile descriptor used to validate data
     * uint32            version;           // version number of packfile
     * char              short_name[V_MAX_PACKFILE_NAME_LEN+1];  // filename - 8.3 format
     * char              pathname[V_PACK_MAX_PATH_LEN+1];        // pathname
     * 
     * uint32            flags;             // packfile flags
     * uint32            sector;            // packfile starts at this sector
     * 
     * uint32            num_files;         // number of files in *data section
     * uint32            file_size;         // physical size (in bytes) of the source vpp file
     * uint32            dir_size;          // number of bytes in directory section
     * uint32            filename_size;     // number of bytes in filename section
     * 
     * uint32            data_size;         // number of uncompressed bytes in data files
     * uint32            compressed_data_size; // number of compressed bytes in *data section
     * 
     * v_packfile_entry  *dir;              // directory section
     * char              *filenames;        // file name section
     * uint8             *data;             // data section -- set gameside when a packfile is wholely loaded into memory (temp, condensed, or memory mapped)
     * 
     * uint32            open_count;        // how many files have open handles into the packfile
     * };
     * 
     * V_MAX_PACKFILE_NAME_LEN = 64
     * V_PACK_MAX_PATH_LEN = 255
     */
    [StructLayout(LayoutKind.Explicit, Size = 0x17C, CharSet = CharSet.Ansi)]
    public struct PackfileFileData
    {
        [FieldOffset(0x000)]
        public UInt32 Descriptor;

        [FieldOffset(0x004)]
        public UInt32 Version;


        // These fields are runtime fields and cause alignment issues
        //[FieldOffset(0x008)]
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        //public string ShortFilename;

        //[FieldOffset(0x049)]
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        //public string Pathname;

        [FieldOffset(0x14C)]
        public PackfileFlags Flags;

        [FieldOffset(0x150)]
        public UInt32 Sector; // Packfile starts at this sector, unused on PC?

        [FieldOffset(0x154)]
        public UInt32 NumFiles;

        [FieldOffset(0x158)]
        public UInt32 PackfileSize;

        [FieldOffset(0x15C)]
        public UInt32 DirectorySize;

        [FieldOffset(0x160)]
        public UInt32 FilenamesSize;

        [FieldOffset(0x164)]
        public UInt32 DataSize;

        [FieldOffset(0x168)]
        public UInt32 CompressedDataSize;

        [FieldOffset(0x16C)]
        public UInt32 DirectoryOffset;

        [FieldOffset(0x170)]
        public UInt32 FilenameOffset;

        [FieldOffset(0x174)]
        public UInt32 DataOffset;

        [FieldOffset(0x178)]
        public UInt32 OpenCount;
    }
}
