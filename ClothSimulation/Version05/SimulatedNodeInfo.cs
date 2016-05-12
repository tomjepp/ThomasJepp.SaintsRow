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
UserDefinedType: simulated_node_info
Data           :   this+0x0, Member, Type: short, bone_index
Data           :   this+0x2, Member, Type: short, parent_node_index
Data           :   this+0x4, Member, Type: short, gravity_link
Data           :   this+0x6, Member, Type: short, anchor
Data           :   this+0x8, Member, Type: unsigned int, collide
Data           :   this+0xC, Member, Type: float, wind_multiplier
Data           :   this+0x10, Member, Type: class vm_vector, pos
    UserDefinedType:     vm_vector
Data           :   this+0x20, Member, Type: class vm_vector, local_space_pos
    UserDefinedType:     vm_vector
    */

    [StructLayout(LayoutKind.Explicit, Size = 0x30)]
    public struct SimulatedNodeInfo
    {
        [FieldOffset(0x00)]
        public Int16 BoneIndex;

        [FieldOffset(0x02)]
        public Int16 ParentNodeIndex;

        [FieldOffset(0x04)]
        public Int16 GravityLink;

        [FieldOffset(0x06)]
        public Int16 Anchor;

        [FieldOffset(0x08)]
        public UInt32 Collide;

        [FieldOffset(0x0C)]
        public float WindMultiplier;

        [FieldOffset(0x10)]
        public SIMDVector128 Pos;

        [FieldOffset(0x20)]
        public SIMDVector128 LocalSpacePos;
    }
}
