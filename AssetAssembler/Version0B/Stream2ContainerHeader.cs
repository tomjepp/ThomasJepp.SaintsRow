using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.AssetAssembler.Version0B
{   
    [StructLayout(LayoutKind.Explicit, Size=0x08)]
    public struct Stream2ContainerHeader
    {
        [FieldOffset(0x00)]
        public UInt32 Signature;

        [FieldOffset(0x04)]
        public UInt16 Version;

        [FieldOffset(0x06)]
        public Int16 NumContainers;
    }
}
