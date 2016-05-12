using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version02
{ 
    [StructLayout(LayoutKind.Explicit, Size = 0x6C, CharSet = CharSet.Ansi)]
    public struct ClothSimulationHeader
    {
        [FieldOffset(0x00)]
        public Int32 Version;

        [FieldOffset(0x04)]
        public Int32 DataSize;

        [FieldOffset(0x08)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
        public string Name;

        [FieldOffset(0x24)]
        public Int32 NumPasses;

        [FieldOffset(0x28)]
        public float AirResistance;

        [FieldOffset(0x2C)]
        public float WindMultiplier;

        [FieldOffset(0x30)]
        public float WindConst;

        [FieldOffset(0x34)]
        public float GravityMultiplier;

        [FieldOffset(0x38)]
        public float ObjectVelocityInheritance;

        [FieldOffset(0x3C)]
        public float ObjectPositionInheritance;

        [FieldOffset(0x40)]
        public float ObjectRotationInheritance;

        [FieldOffset(0x44)]
        public Int32 WindType;

        [FieldOffset(0x48)]
        public Int32 NumNodes;

        [FieldOffset(0x4C)]
        public Int32 NumAnchorNodes;

        [FieldOffset(0x50)]
        public Int32 NumNodeLinks;

        [FieldOffset(0x54)]
        public Int32 NumRopes;

        [FieldOffset(0x58)]
        public Int32 NumColliders;

        [FieldOffset(0x5C)]
        public UInt32 NodesPtr;

        [FieldOffset(0x60)]
        public UInt32 NodeLinksPtr;

        [FieldOffset(0x64)]
        public UInt32 RopesPtr;

        [FieldOffset(0x68)]
        public UInt32 CollidersPtr;
    }
}
