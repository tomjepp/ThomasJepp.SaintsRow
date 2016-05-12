using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.AssetAssembler
{
    public interface IAssetAssemblerFile
    {
        uint Version { get; }

        Dictionary<byte, string> AllocatorTypes { get; }
        Dictionary<byte, string> PrimitiveTypes { get; }
        Dictionary<byte, string> ContainerTypes { get; }

        List<IContainer> Containers { get; }

        IContainer CreateContainer();
        void Save(Stream stream);
    }
}
