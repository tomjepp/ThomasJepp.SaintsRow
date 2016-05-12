using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.MiscTypes
{
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct FLQuaternion
    {
        [FieldOffset(0x00)]
        public float X;

        [FieldOffset(0x04)]
        public float Y;

        [FieldOffset(0x08)]
        public float Z;

        [FieldOffset(0x0C)]
        public float W;
    }
}
