using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x0C, CharSet = CharSet.Ansi)]
    public struct RenderLibMaterialTextureDesc
    {
        [FieldOffset(0x00)]
        public int TextureHandle;

        [FieldOffset(0x04)]
        public uint NameChecksum;

        [FieldOffset(0x08)]
        public ushort TextureStage;

        [FieldOffset(0x0A)]
        public ushort TextureFlags;
    }
}
