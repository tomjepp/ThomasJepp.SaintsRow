using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Stream2
{
    public class Stream2File
    {
        private Stream2ContainerHeader Header;

        private Dictionary<byte, string> AllocatorTypes = new Dictionary<byte, string>();
        private Dictionary<byte, string> PrimitiveTypes = new Dictionary<byte, string>();
        private Dictionary<byte, string> ContainerTypes = new Dictionary<byte, string>();

        public Stream2File(Stream stream)
        {
            Header = stream.ReadStruct<Stream2ContainerHeader>();

            uint allocatorTypeCount = stream.ReadUInt32();
            for (uint i = 0; i < allocatorTypeCount; i++)
            {
                UInt16 stringLength = stream.ReadUInt16();
                string name = stream.ReadAsciiString(stringLength);
                byte id = stream.ReadUInt8();
                AllocatorTypes.Add(id, name);
            }

            uint primitiveTypeCount = stream.ReadUInt32();
            for (uint i = 0; i < primitiveTypeCount; i++)
            {
                UInt16 stringLength = stream.ReadUInt16();
                string name = stream.ReadAsciiString(stringLength);
                byte id = stream.ReadUInt8();
                PrimitiveTypes.Add(id, name);
            }

            uint containerTypeCount = stream.ReadUInt32();
            for (uint i = 0; i < containerTypeCount; i++)
            {
                UInt16 stringLength = stream.ReadUInt16();
                string name = stream.ReadAsciiString(stringLength);
                byte id = stream.ReadUInt8();
                ContainerTypes.Add(id, name);
            }

            Console.WriteLine("0x{0:X8} - {0}", stream.Position);
        }
    }
}
