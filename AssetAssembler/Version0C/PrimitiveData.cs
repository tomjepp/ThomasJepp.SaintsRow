using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.AssetAssembler.Version0C
{
    [StructLayout(LayoutKind.Explicit, Size = 0x0D)]
    public struct PrimitiveData
    {
        [FieldOffset(0x00)]
        public byte Type;

        [FieldOffset(0x01)]
        public byte Allocator;

        [FieldOffset(0x02)]
        public byte Flags;

        [FieldOffset(0x03)]
        public byte ExtensionIndex;

        [FieldOffset(0x04)]
        public UInt32 CPUSize;

        [FieldOffset(0x08)]
        public UInt32 GPUSize;

        [FieldOffset(0x0C)]
        public byte AllocationGroup;
    }
}
