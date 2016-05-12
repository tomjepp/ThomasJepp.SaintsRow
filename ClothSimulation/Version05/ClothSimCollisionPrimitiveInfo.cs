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
UserDefinedType: cloth_sim_collision_primitive_info
Data           :   this+0x0, Member, Type: int, bone_index
Data           :   this+0x4, Member, Type: short, is_capsule
Data           :   this+0x6, Member, Type: short, do_scale
Data           :   this+0x8, Member, Type: float, radius
Data           :   this+0xC, Member, Type: float, height
Data           :   this+0x10, Member, Type: class vm_vector, pos
    UserDefinedType:     vm_vector

Data           :   this+0x20, Member, Type: class vm_vector, axis
    UserDefinedType:     vm_vector

Data           :   this+0x30, Member, Type: class vm_vector, local_space_pos
    UserDefinedType:     vm_vector
    */

    [StructLayout(LayoutKind.Explicit, Size = 0x40)]
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
        public SIMDVector128 Pos;

        [FieldOffset(0x20)]
        public SIMDVector128 Axis;

        [FieldOffset(0x30)]
        public SIMDVector128 LocalSpacePos;
    }
}
