using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ThomasJepp.SaintsRow.MiscTypes;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version02
{
    [StructLayout(LayoutKind.Explicit, Size = 0x34)]
    public struct ClothSimCollisionPrimitiveInfo
    {
        [FieldOffset(0x00)]
        public Int32 BoneIndex;

        [FieldOffset(0x04)]
        public Int16 IsCapsule;

        [FieldOffset(0x06)]
        public Int16 DoScale;

        [FieldOffset(0x08)]
        public float Radius;

        [FieldOffset(0x0C)]
        public float Height;

        [FieldOffset(0x10)]
        public FLVector Pos;

        [FieldOffset(0x1C)]
        public FLVector Axis;

        [FieldOffset(0x28)]
        public FLVector LocalSpacePos;
    }
}
