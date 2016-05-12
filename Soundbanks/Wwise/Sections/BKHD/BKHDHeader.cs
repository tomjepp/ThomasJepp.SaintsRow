using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.BKHD
{
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct BKHDHeader
    {
        [FieldOffset(0x00)]
        public UInt32 Version;

        [FieldOffset(0x04)]
        public UInt32 ID;

        [FieldOffset(0x08)]
        public Int32 Zero1;

        [FieldOffset(0x0C)]
        public Int32 Zero2;
    }
}
