using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x18, CharSet = CharSet.Ansi)]
    public struct CMeshCSphere
    {
        [FieldOffset(0x00)]
        public UInt32 BodyPartId;

        [FieldOffset(0x04)]
        public Int32 ParentIndex;

        [FieldOffset(0x08)]
        public FLVector Position;

        [FieldOffset(0x14)]
        public float Radius;
    }
}
