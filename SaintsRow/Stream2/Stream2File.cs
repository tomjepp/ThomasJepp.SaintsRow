using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Stream2
{
    public class Stream2File
    {
        public ContainerFileHeader Header { get; set; }

        public Dictionary<byte, string> AllocatorTypes = new Dictionary<byte, string>();
        public Dictionary<byte, string> PrimitiveTypes = new Dictionary<byte, string>();
        public Dictionary<byte, string> ContainerTypes = new Dictionary<byte, string>();

        public List<Container> Containers = new List<Container>();

        public Stream2File(Stream stream)
        {
            Header = stream.ReadStruct<ContainerFileHeader>();

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

            for (uint i = 0; i < Header.NumContainers; i++)
            {
                Container container = new Container(stream);
                Containers.Add(container);
            }
        }

        public void Save(Stream stream)
        {
            stream.WriteStruct(Header);

            // Write allocator types
            stream.WriteUInt32((uint)AllocatorTypes.Count);
            foreach (var pair in AllocatorTypes)
            {
                byte id = pair.Key;
                string name = pair.Value;
                stream.WriteUInt16((UInt16)name.Length);
                stream.WriteAsciiString(name);
                stream.WriteUInt8(id);
            }

            // Write primitive types
            stream.WriteUInt32((uint)PrimitiveTypes.Count);
            foreach (var pair in PrimitiveTypes)
            {
                byte id = pair.Key;
                string name = pair.Value;
                stream.WriteUInt16((UInt16)name.Length);
                stream.WriteAsciiString(name);
                stream.WriteUInt8(id);
            }

            // Write container types
            stream.WriteUInt32((uint)ContainerTypes.Count);
            foreach (var pair in ContainerTypes)
            {
                byte id = pair.Key;
                string name = pair.Value;
                stream.WriteUInt16((UInt16)name.Length);
                stream.WriteAsciiString(name);
                stream.WriteUInt8(id);
            }

            // Write containers
            foreach (Container container in Containers)
            {
                container.Save(stream);
            }
        }
    }
}
