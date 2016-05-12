using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ThomasJepp.SaintsRow.MiscTypes;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version05
{
    /*
UserDefinedType: simulated_node_link_info
Data           :   this+0x0, Member, Type: unsigned short, node_index_1
Data           :   this+0x2, Member, Type: unsigned short, node_index_2
Data           :   this+0x4, Member, Type: unsigned int, collide
Data           :   this+0x8, Member, Type: float, length
Data           :   this+0xC, Member, Type: float, stretch_len
Data           :   this+0x10, Member, Type: float, twist
Data           :   this+0x14, Member, Type: float, spring
Data           :   this+0x18, Member, Type: float, damp
Data           :   this+0x1C, Member, Type: bool, is_gravity_link

    */

    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
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

        [MarshalAs(UnmanagedType.Bool)]
        [FieldOffset(0x1C)]
        public bool IsGravityLink;
    }
}
