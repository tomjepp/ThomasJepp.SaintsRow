using System;
using System.Runtime.InteropServices;
using ThomasJepp.SaintsRow.MiscTypes;


namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 0x88, CharSet = CharSet.Ansi)]
    public struct StaticMeshHeader
    {
        [FieldOffset(0x00)]
        public Int32 Signature;

        [FieldOffset(0x04)]
        public Int16 Version;

        [FieldOffset(0x06)]
        public Int16 MeshFlags;

        [FieldOffset(0x08)]
        public Int16 NumNavpoints;

        [FieldOffset(0x0A)]
        public Int16 NumRigBones;

        [FieldOffset(0x0C)]
        public Int16 NumMaterials;

        [FieldOffset(0x0E)]
        public Int16 NumMaterialMaps;

        [FieldOffset(0x10)]
        public Int16 NumLODsPerSubmesh;

        [FieldOffset(0x12)]
        public UInt16 NumSubmeshVIDs;

        [FieldOffset(0x18)]
        public VWidePtrUInt32 Navpoints;

        [FieldOffset(0x20)]
        public VWidePtrUInt32 RigBoneIndicies;

        [FieldOffset(0x28)]
        public FLVector BoundingCenter;

        [FieldOffset(0x34)]
        public Single BoundingRadius;

        [FieldOffset(0x38)]
        public UInt16 NumCSpheres;

        [FieldOffset(0x3A)]
        public UInt16 NumCCylinders;

        [FieldOffset(0x40)]
        public VWidePtrUInt32 CSpheres;

        [FieldOffset(0x48)]
        public VWidePtrUInt32 CCylinders;

        [FieldOffset(0x50)]
        public VWidePtrUInt32 Mesh;

        [FieldOffset(0x58)]
        public VWidePtrUInt32 MaterialMaps;

        [FieldOffset(0x60)]
        public VWidePtrUInt32 MaterialMapNameCRCs;

        [FieldOffset(0x68)]
        public VWidePtrUInt32 Materials;

        [FieldOffset(0x70)]
        public UInt32 NumLogicalSubmeshes;

        [FieldOffset(0x78)]
        public VWidePtrUInt32 SubmeshVIDs;

        [FieldOffset(0x80)]
        public VWidePtrUInt32 SubmeshLODInfo;
    }
}
