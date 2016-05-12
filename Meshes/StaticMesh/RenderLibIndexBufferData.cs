using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x18, CharSet = CharSet.Ansi)]
    public struct RenderLibIndexBufferData
    {
        [FieldOffset(0x00)]
        public uint NumIndicies;

        [FieldOffset(0x08)]
        public VWidePtrUInt32 Indicies;

        [FieldOffset(0x10)]
        public byte IndexSize;

        [FieldOffset(0x11)]
        public byte PrimitiveType;

        [FieldOffset(0x12)]
        public ushort NumBlocks;
    }
}
