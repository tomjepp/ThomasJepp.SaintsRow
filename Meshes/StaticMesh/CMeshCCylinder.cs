using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x28, CharSet = CharSet.Ansi)]
    public struct CMeshCCylinder
    {
        [FieldOffset(0x00)]
        public UInt32 BodyPartId;

        [FieldOffset(0x04)]
        public Int32 ParentIndex;

        [FieldOffset(0x08)]
        public FLVector Axis;

        [FieldOffset(0x14)]
        public FLVector Position;

        [FieldOffset(0x20)]
        public float Radius;

        [FieldOffset(0x24)]
        public float Height;
    }
}
