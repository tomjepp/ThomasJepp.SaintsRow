using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.AssetAssembler
{
    public interface IPrimitive
    {
        string Name { get; set; }
        byte Type { get; set; }
        byte Allocator { get; set; }
        byte Flags { get; set; }
        byte ExtensionIndex { get; set; }
        byte AllocationGroup { get; set; }
        UInt32 CPUSize { get; set; }
        UInt32 GPUSize { get; set; }
    }
}
