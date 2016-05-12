using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x20, CharSet = CharSet.Ansi)]
    public struct RenderLibBoneMapData
    {
        [FieldOffset(0x00)]
        public ushort NumMappedBones;

        [FieldOffset(0x08)]
        public VWidePtrUInt32 MappedBoneList;

        [FieldOffset(0x10)]
        public ushort NumBoneGroups;

        [FieldOffset(0x18)]
        public VWidePtrUInt32 BoneGroupList;
    }
}
