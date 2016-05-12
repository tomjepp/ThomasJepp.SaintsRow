using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x30, CharSet = CharSet.Ansi)]
    public struct RenderLibMaterialData
    {
        [FieldOffset(0x00)]
        public uint ShaderHandle;

        [FieldOffset(0x04)]
        public uint NameChecksum;

        [FieldOffset(0x08)]
        public uint MaterialFlags;

        [FieldOffset(0x0C)]
        public ushort NumTextures;

        [FieldOffset(0x0E)]
        public byte NumConstants;

        [FieldOffset(0x10)]
        public VWidePtrUInt32 Textures;

        [FieldOffset(0x18)]
        public VWidePtrUInt32 ConstantNameChecksum;

        [FieldOffset(0x20)]
        public VWidePtrUInt32 ConstantBlock;

        [FieldOffset(0x28)]
        public uint AlphaShaderHandle;

    }
}
