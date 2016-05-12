using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ThomasJepp.SaintsRow.MiscTypes;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version02
{

    [StructLayout(LayoutKind.Explicit, Size = 0x1C)]
    public struct SimulatedNodeLinkInfo
    {
        [FieldOffset(0x00)]
        public UInt16 NodeIndex1;

        [FieldOffset(0x02)]
        public UInt16 NodeIndex2;

        [FieldOffset(0x04)]
        public UInt32 Collide;

        [FieldOffset(0x08)]
        public float Length;

        [FieldOffset(0x0C)]
        public float StretchLen;

        [FieldOffset(0x10)]
        public float Twist;

        [FieldOffset(0x14)]
        public float Spring;

        [FieldOffset(0x18)]
        public float Damp;
    }
}
