using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Bitmaps.Version13
{
    [StructLayout(LayoutKind.Explicit, Size = 0x48, CharSet = CharSet.Ansi)]
    public struct PegEntryData
    { 
        [FieldOffset(0x00)]
        public VWidePtrUInt32 DataPtr;

        [FieldOffset(0x08)]
        public ushort Width;

        [FieldOffset(0x0A)]
        public ushort Height;

        [FieldOffset(0x0C)]
        public ushort BitmapFormat;

        [FieldOffset(0x0E)]
        public ushort PaletteFormat;

        [FieldOffset(0x10)]
        public ushort AnimTilesWidth;

        [FieldOffset(0x12)]
        public ushort AnimTilesHeight;

        [FieldOffset(0x14)]
        public ushort NumFrames;

        [FieldOffset(0x16)]
        public ushort Flags;

        [FieldOffset(0x18)]
        public VWidePtrUInt32 FilenamePtr;

        [FieldOffset(0x20)]
        public ushort PaletteSize;

        [FieldOffset(0x22)]
        public byte FramesPerSecond;

        [FieldOffset(0x23)]
        public byte MipmapLevels;

        [FieldOffset(0x24)]
        public uint FrameSize;

        [FieldOffset(0x28)]
        public VWidePtrUInt32 Next;

        [FieldOffset(0x30)]
        public VWidePtrUInt32 Prev;

        [FieldOffset(0x38)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ulong[] Cache;
    }
}
