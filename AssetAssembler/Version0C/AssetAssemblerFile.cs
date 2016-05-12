using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.AssetAssembler.Version0C
{
    public class AssetAssemblerFile : IAssetAssemblerFile
    {
        public Stream2ContainerHeader Header;

        public uint Version
        {
            get
            {
                return Header.Version;
            }
        }

        public Dictionary<byte, string> AllocatorTypes { get; set; }
        public Dictionary<byte, string> PrimitiveTypes { get; set; }
        public Dictionary<byte, string> ContainerTypes { get; set; }

        public List<IContainer> Containers { get; set; }


        public AssetAssemblerFile()
        {
            Header = new Stream2ContainerHeader();
            AllocatorTypes = new Dictionary<byte, string>();
            PrimitiveTypes = new Dictionary<byte, string>();
            ContainerTypes = new Dictionary<byte, string>();
            Containers = new List<IContainer>();
        }

        public AssetAssemblerFile(Stream stream)
        {
            Header = stream.ReadStruct<Stream2ContainerHeader>();
            AllocatorTypes = new Dictionary<byte, string>();
            PrimitiveTypes = new Dictionary<byte, string>();
            ContainerTypes = new Dictionary<byte, string>();
            Containers = new List<IContainer>();

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

        public IContainer CreateContainer()
        {
            return new Container();
        }

        public void Save(Stream stream)
        {
            Header.Signature = (uint)0xBEEFFEED;
            Header.Version = (ushort)0x000C;
            Header.NumContainers = (short)Containers.Count;

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
