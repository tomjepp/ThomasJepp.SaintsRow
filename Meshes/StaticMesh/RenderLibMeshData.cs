using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x70, CharSet = CharSet.Ansi)]
    public struct RenderLibMeshData
    {
        [FieldOffset(0x00)]
        public uint MeshFlags;

        [FieldOffset(0x04)]
        public int NumSubMeshes;

        [FieldOffset(0x08)]
        public VWidePtrUInt32 Submeshes;

        [FieldOffset(0x10)]
        public uint NumVertexBuffers;

        [FieldOffset(0x18)]
        public VWidePtrUInt32 VertexBuffers;

        [FieldOffset(0x20)]
        public RenderLibIndexBufferData IndexBuffer;

        [FieldOffset(0x38)]
        public RenderLibBoneMapData BoneMap;

        [FieldOffset(0x58)]
        public FLVector PositionScale;

        [FieldOffset(0x64)]
        public FLVector PositionOffset;
    }
}
