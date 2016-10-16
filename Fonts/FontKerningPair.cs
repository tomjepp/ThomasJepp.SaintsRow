using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Fonts
{
    [StructLayout(LayoutKind.Explicit, Size = 0x06)]
    public struct FontKerningPair
    {
        [FieldOffset(0x00)]
        public ushort Char1;
        [FieldOffset(0x02)]
        public ushort Char2;
        [FieldOffset(0x04)]
        public sbyte Offset;
        [FieldOffset(0x05)]
        public byte Padding;
    }
}
