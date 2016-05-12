using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.AssetAssembler.Version0B
{
    [StructLayout(LayoutKind.Explicit, Size = 0x08)]
    public struct WriteTimeSizes
    {
        [FieldOffset(0x00)]
        public UInt32 CPUSize;

        [FieldOffset(0x04)]
        public UInt32 GPUSize;
    }
}
