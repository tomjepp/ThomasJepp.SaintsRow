using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ThomasJepp.SaintsRow.MiscTypes;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version02
{
    [StructLayout(LayoutKind.Explicit, Size = 0x14)]
    public struct ClothSimRopeInfo
    {
        [FieldOffset(0x00)]
        public float Length;

        [FieldOffset(0x04)]
        public int NumNodes;

        [FieldOffset(0x08)]
        public int NumLinks;

        [FieldOffset(0x0C)]
        public UInt32 NodeIndecies;

        [FieldOffset(0x10)]
        public UInt32 LinkIndecies;
    }
}
