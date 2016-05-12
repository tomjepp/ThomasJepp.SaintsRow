using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version05
{
    /*
UserDefinedType: cloth_sim_info
Data           :   this+0x0, Member, Type: int, version
Data           :   this+0x4, Member, Type: int, data_size
Data           :   this+0x8, Member, Type: char[0x1C], name
Data           :   this+0x24, Member, Type: int, num_passes
Data           :   this+0x28, Member, Type: float, air_resistance
Data           :   this+0x2C, Member, Type: float, wind_multiplier
Data           :   this+0x30, Member, Type: float, wind_const
Data           :   this+0x34, Member, Type: float, gravity_multiplier
Data           :   this+0x38, Member, Type: float, object_velocity_inheritance
Data           :   this+0x3C, Member, Type: float, object_position_inheritance
Data           :   this+0x40, Member, Type: float, object_rotation_inheritance
Data           :   this+0x44, Member, Type: int, wind_type
Data           :   this+0x48, Member, Type: int, num_nodes
Data           :   this+0x4C, Member, Type: int, num_anchor_nodes
Data           :   this+0x50, Member, Type: int, num_node_links
Data           :   this+0x54, Member, Type: int, num_ropes
Data           :   this+0x58, Member, Type: int, num_colliders
Data           :   this+0x5C, Member, Type: float, bounding_sphere_radius
Data           :   this+0x60, Member, Type: struct v_wide_ptr_type<simulated_node_info *>, nodes
    UserDefinedType:     v_wide_ptr_type<simulated_node_info *>
Data           :   this+0x68, Member, Type: struct v_wide_ptr_type<simulated_node_link_info *>, node_links
    UserDefinedType:     v_wide_ptr_type<simulated_node_link_info *>
Data           :   this+0x70, Member, Type: struct v_wide_ptr_type<cloth_sim_rope_info *>, ropes
UserDefinedType:     v_wide_ptr_type<cloth_sim_rope_info *>
Data           :   this+0x78, Member, Type: struct v_wide_ptr_type<cloth_sim_collision_primitive_info *>, colliders
    UserDefinedType:     v_wide_ptr_type<cloth_sim_collision_primitive_info *>
    */

    [StructLayout(LayoutKind.Explicit, Size = 0x80, CharSet = CharSet.Ansi)]
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
        public float BoundingSphereRadius;

        [FieldOffset(0x60)]
        public UInt64 NodesPtr;

        [FieldOffset(0x68)]
        public UInt64 NodeLinksPtr;

        [FieldOffset(0x70)]
        public UInt64 RopesPtr;

        [FieldOffset(0x78)]
        public UInt64 CollidersPtr;
    }
}
