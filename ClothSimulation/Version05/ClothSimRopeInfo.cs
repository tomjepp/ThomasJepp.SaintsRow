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
UserDefinedType: cloth_sim_rope_info
Data           :   this+0x0, Member, Type: float, length
Data           :   this+0x4, Member, Type: int, num_nodes
Data           :   this+0x8, Member, Type: int, num_links
Data           :   this+0x10, Member, Type: struct v_wide_ptr_type<int *>, node_indecies
UserDefinedType:     v_wide_ptr_type<int *>

Data           :   this+0x18, Member, Type: struct v_wide_ptr_type<int *>, link_indecies
UserDefinedType:     v_wide_ptr_type<int *>
    */

    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public struct ClothSimRopeInfo
    {
        [FieldOffset(0x00)]
        public float Length;

        [FieldOffset(0x04)]
        public int NumNodes;

        [FieldOffset(0x08)]
        public int NumLinks;

        [FieldOffset(0x10)]
        public UInt64 NodeIndecies;

        [FieldOffset(0x18)]
        public UInt64 LinkIndecies;
    }
}
