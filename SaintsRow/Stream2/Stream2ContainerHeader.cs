using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Stream2
{
    /*
     * UserDefinedType: stream2_container_header
     * Data           :   this+0x0, Member, Type: unsigned int, signature
     * Data           :   this+0x4, Member, Type: unsigned short, version
     * Data           :   this+0x6, Member, Type: unsigned short, num_containers
     */
    
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
