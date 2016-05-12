using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.AssetAssembler
{
    public interface IContainer
    {
        string Name { get; set; }
        byte ContainerType { get; set; }
        ContainerFlags Flags { get; set; }
        Int16 PrimitiveCount { get; set; }
        UInt32 PackfileBaseOffset { get; set; } // Data start offset in str2
        byte CompressionType { get; set; } // Always 0x09?
        string StubContainerParentName { get; set; }
        byte[] AuxData { get; set; }
        Int32 TotalCompressedPackfileReadSize { get; set; } // Size of condensed data

        List<IPrimitive> Primitives { get; }

        IPrimitive CreatePrimitive();
    }
}
