using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Meshes.StaticMesh
{
    public class StaticMeshMaterial
    {
        public uint DataSize;
        public RenderLibMaterialData Data = new RenderLibMaterialData();
        public List<RenderLibMaterialTextureDesc> TextureDescriptions = new List<RenderLibMaterialTextureDesc>();
        public List<uint> NameChecksums = new List<uint>();
        public List<RenderLibMaterialConstants> Constants = new List<RenderLibMaterialConstants>();
        public List<string> TextureNames = new List<string>();
    }
}
