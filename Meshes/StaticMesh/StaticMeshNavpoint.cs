using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x60, CharSet = CharSet.Ansi)]
    public struct StaticMeshNavpoint
    {
        [FieldOffset(0x00)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;

        [FieldOffset(0x40)]
        public Int32 VID;

        [FieldOffset(0x44)]
        public FLVector Position;

        [FieldOffset(0x50)]
        public FLQuaternion Orientation;
    }
}
