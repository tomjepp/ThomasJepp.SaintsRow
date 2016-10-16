using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Fonts
{
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct FontCharacter
    {
        [FieldOffset(0x00)]
        public int Spacing;
        [FieldOffset(0x04)]
        public int ByteWidth;
        [FieldOffset(0x08)]
        public int Offset;
        [FieldOffset(0x0C)]
        public short KerningEntry;
        [FieldOffset(0x0E)]
        public short UserData;
    }
}
