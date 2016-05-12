using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    public class StaticMesh
    {
        private StaticMeshHeader Header = new StaticMeshHeader();
        public Dictionary<string, int> TextureNames = new Dictionary<string, int>();
        public List<StaticMeshNavpoint> Navpoints = new List<StaticMeshNavpoint>();
        public List<CMeshCSphere> CSpheres = new List<CMeshCSphere>();
        public List<CMeshCCylinder> CCylinders = new List<CMeshCCylinder>();
        public List<UInt32> RigBones = new List<UInt32>();

        public RenderLibMeshData MeshData = new RenderLibMeshData();
        public List<StaticMeshMaterial> Materials = new List<StaticMeshMaterial>();

        public ushort MeshVersion;
        public uint MeshGpuDataCrc;
        public uint MeshCpuDataSize;

        public byte[] MiscData;
        public byte[] MaterialMapData;

        public StaticMesh()
        {
        }

        public StaticMesh(Stream s)
        {
            s.Align(4);

            s.Align(8);

            Header = s.ReadStruct<StaticMeshHeader>();

            // Read texture names
            int textureNameSize = s.ReadInt32();
            s.Align(16);

            s.ReadByte();

            byte[] textureNameData = new byte[textureNameSize];
            s.Read(textureNameData, 0, textureNameSize);
            MemoryStream textureNameStream = new MemoryStream(textureNameData);

            while (textureNameStream.Position < textureNameData.Length)
            {
                textureNameStream.Align(2);

                int position = (int)textureNameStream.Position;
                string textureName = textureNameStream.ReadAsciiNullTerminatedString();
                byte paddingByte = textureNameStream.ReadUInt8();

                TextureNames.Add(textureName, position);

                if (paddingByte != 0)
                    throw new Exception();

                textureNameStream.Align(2);
            }

            if (Header.NumNavpoints > 0)
            {
                s.Align(16);

                for (int i = 0; i < Header.NumNavpoints; i++)
                {
                    StaticMeshNavpoint navpoint = s.ReadStruct<StaticMeshNavpoint>();
                    Navpoints.Add(navpoint);
                }
            }

            if (Header.NumCSpheres > 0)
            {
                s.Align(16);

                for (int  i = 0; i < Header.NumCSpheres; i++)
                {
                    CMeshCSphere cmeshCSphere = s.ReadStruct<CMeshCSphere>();
                    CSpheres.Add(cmeshCSphere);
                }
            }

            if (Header.NumCCylinders > 0)
            {
                s.Align(16);

                for (int i = 0; i < Header.NumCCylinders; i++)
                {
                    CMeshCCylinder cylinder = s.ReadStruct<CMeshCCylinder>();
                    CCylinders.Add(cylinder);
                }
            }

            if (Header.NumRigBones > 0)
            {
                s.Align(16);

                for (int i = 0; i < Header.NumRigBones; i++)
                {
                    UInt32 rigBone = s.ReadUInt32();
                    RigBones.Add(rigBone);
                }
            }

            s.Align(8);

            long originalOffset = s.Position;

            MeshVersion = s.ReadUInt16();

            s.Align(4);

            MeshGpuDataCrc = s.ReadUInt32();
            MeshCpuDataSize = s.ReadUInt32();

            MiscData = new byte[(originalOffset + MeshCpuDataSize) - s.Position];
            s.Read(MiscData, 0, MiscData.Length);

            if (Header.NumMaterialMaps > 0)
            {
                s.Align(8);

                // Skip pointer tables?
                s.Seek(8 * Header.NumMaterialMaps, SeekOrigin.Current);
                s.Seek(4 * Header.NumMaterialMaps, SeekOrigin.Current);
            }

            if (Header.NumMaterials > 0)
            {
                s.Align(8);

                // Skip more pointers
                s.Seek(8 * Header.NumMaterials, SeekOrigin.Current);

                for (int i = 0; i < Header.NumMaterials; i++)
                {
                    StaticMeshMaterial material = new StaticMeshMaterial();
                    Materials.Add(material);
                    material.DataSize = s.ReadUInt32();

                    s.Align(8);

                    material.Data = s.ReadStruct<RenderLibMaterialData>();

                    for (int j = 0; j < material.Data.NumTextures; j++)
                    {
                        RenderLibMaterialTextureDesc textureDesc = s.ReadStruct<RenderLibMaterialTextureDesc>();
                        material.TextureDescriptions.Add(textureDesc);
                    }

                    for (int j = 0; j < material.Data.NumConstants; j++)
                    {
                        uint constantNameChecksum = s.ReadUInt32();
                        material.NameChecksums.Add(constantNameChecksum);
                    }

                    s.Align(16);

                    for (int j = 0; j < material.Data.NumConstants; j++)
                    {
                        RenderLibMaterialConstants constant = s.ReadStruct<RenderLibMaterialConstants>();
                        material.Constants.Add(constant);
                    }

                    if (material.Data.NumTextures > 0)
                    {
                        for (int j = 0; j < material.Data.NumTextures; j++)
                        {
                            RenderLibMaterialTextureDesc textureDesc =  material.TextureDescriptions[j];
                            textureNameStream.Seek(textureDesc.TextureHandle, SeekOrigin.Begin);
                            string textureName = textureNameStream.ReadAsciiNullTerminatedString();
                            material.TextureNames.Add(textureName);
                        }
                    }
                }
            }

            MaterialMapData = new byte[s.Length - s.Position];
            s.Read(MaterialMapData, 0, MaterialMapData.Length);
        }

        public void RenameTexture(string oldName, string newName)
        {
            TextureNames.Remove(oldName);
            TextureNames.Add(newName, 0);

            foreach (StaticMeshMaterial material in Materials)
            {
                for (int i = 0; i < material.Data.NumTextures; i++)
                {
                    if (material.TextureNames[i] == oldName)
                    {
                        material.TextureNames[i] = newName;
                    }
                }
            }
        }

        public void Save(Stream s)
        {
            s.Align(4);

            s.Align(8);

            Header.NumNavpoints = (short)Navpoints.Count;
            Header.NumRigBones = (short)RigBones.Count;
            // Header.NumMaterials
            // Header.NumMaterialMaps
            // Header.NumLODsPerSubmesh
            // Header.NumSubmeshVIDs
            Header.NumCSpheres = (ushort)CSpheres.Count;
            Header.NumCCylinders = (ushort)CCylinders.Count;
            // NumLogicalSubmeshes

            s.WriteStruct(Header);

            using (MemoryStream textureNameStream = new MemoryStream())
            {
                string[] textureNames = new string[TextureNames.Count];
                TextureNames.Keys.CopyTo(textureNames, 0);
                foreach (var textureName in textureNames)
                {
                    textureNameStream.Align(2);
                    TextureNames[textureName] = (int)textureNameStream.Position;
                    textureNameStream.WriteAsciiNullTerminatedString(textureName);
                    textureNameStream.WriteUInt8(0); // padding byte?
                    textureNameStream.Align(2);
                }

                s.WriteInt32((int)textureNameStream.Length);
                s.Align(16);
                s.WriteUInt8(0);
                textureNameStream.Seek(0, SeekOrigin.Begin);
                textureNameStream.CopyTo(s);
            }

            if (Header.NumNavpoints > 0)
            {
                s.Align(16);

                foreach (StaticMeshNavpoint navpoint in Navpoints)
                {
                    s.WriteStruct(navpoint);
                }
            }

            if (Header.NumCSpheres > 0)
            {
                s.Align(16);

                foreach (CMeshCSphere csphere in CSpheres)
                {
                    s.WriteStruct(csphere);
                }
            }

            if (Header.NumCCylinders > 0)
            {
                s.Align(16);

                foreach (CMeshCCylinder ccylinder in CCylinders)
                {
                    s.WriteStruct(ccylinder);
                }
            }

            if (Header.NumRigBones > 0)
            {
                s.Align(16);

                foreach (uint rigBone in RigBones)
                {
                    s.WriteUInt32(rigBone);
                }
            }

            s.Align(8);

            s.WriteUInt16(MeshVersion);

            s.Align(4);
            s.WriteUInt32(MeshGpuDataCrc);
            s.WriteUInt32(MeshCpuDataSize);

            s.Write(MiscData, 0, MiscData.Length);

            if (Header.NumMaterialMaps > 0)
            {
                s.Align(8);

                // Skip pointer tables?
                s.Seek(8 * Header.NumMaterialMaps, SeekOrigin.Current);
                s.Seek(4 * Header.NumMaterialMaps, SeekOrigin.Current);
            }

            if (Header.NumMaterials > 0)
            {
                s.Align(8);

                // Skip more pointers
                s.Seek(8 * Header.NumMaterials, SeekOrigin.Current);

                for (int i = 0; i < Header.NumMaterials; i++)
                {
                    StaticMeshMaterial material = Materials[i];
                    s.WriteUInt32(material.DataSize);

                    s.Align(8);
                    s.WriteStruct(material.Data);

                    for (int j = 0; j < material.Data.NumTextures; j++)
                    {
                        string textureName = material.TextureNames[j];
                        RenderLibMaterialTextureDesc textureDesc = material.TextureDescriptions[j];
                        int textureNameOffset = TextureNames[textureName];
                        textureDesc.TextureHandle = textureNameOffset;

                        s.WriteStruct(textureDesc);
                    }

                    for (int j = 0; j < material.Data.NumConstants; j++)
                    {
                        uint constantNameChecksum = material.NameChecksums[j];
                        s.WriteUInt32(constantNameChecksum);
                    }

                    s.Align(16);

                    for (int j = 0; j < material.Data.NumConstants; j++)
                    {
                        RenderLibMaterialConstants constant = material.Constants[j];
                        s.WriteStruct(constant);
                    }
                }
            }

            s.Write(MaterialMapData, 0, MaterialMapData.Length);
        }
    }
}
