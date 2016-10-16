using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ThomasJepp.SaintsRow.MiscTypes;

namespace ThomasJepp.SaintsRow.Fonts
{
    [StructLayout(LayoutKind.Explicit, Size = 0xD0)]
    public struct FontHeader
    {
        [FieldOffset(0x00)]
        public int ID;
        [FieldOffset(0x04)]
        public int Version;
        [FieldOffset(0x08)]
        public int NumberOfCharacters;
        [FieldOffset(0x0C)]
        public int FirstAscii;
        [FieldOffset(0x10)]
        public int Width;
        [FieldOffset(0x14)]
        public short Height;
        [FieldOffset(0x16)]
        public short RenderHeight;
        [FieldOffset(0x18)]
        public int BaselineOffset;
        [FieldOffset(0x1C)]
        public int CharacterSpacing;
        [FieldOffset(0x20)]
        public int NumberOfKerningPairs;
        [FieldOffset(0x24)]
        public short VerticalOffset;
        [FieldOffset(0x26)]
        public short HorizontalOffset;
        [FieldOffset(0x28)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
        public string PegName;
        [FieldOffset(0x68)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
        public string BitmapName;
        [FieldOffset(0xA8)]
        public VWidePtrUInt32 KerningData;
        [FieldOffset(0xB0)]
        public VWidePtrUInt32 CharacterData;
        [FieldOffset(0xB8)]
        public int BitmapID;
        [FieldOffset(0xC0)]
        public VWidePtrUInt32 bm_u;
        [FieldOffset(0xC8)]
        public VWidePtrUInt32 bm_v;
    }
}
