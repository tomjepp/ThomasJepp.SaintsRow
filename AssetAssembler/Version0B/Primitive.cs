using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.AssetAssembler.Version0B
{
    public class Primitive : IPrimitive
    {
        public string Name { get; set; }

        public byte Type
        {
            get
            {
                return Data.Type;
            }
            set
            {
                Data.Type = value;
            }
        }

        public byte Allocator
        {
            get
            {
                return Data.Allocator;
            }
            set
            {
                Data.Allocator = value;
            }
        }

        public byte Flags
        {
            get
            {
                return Data.Flags;
            }
            set
            {
                Data.Flags = value;
            }
        }

        public byte ExtensionIndex
        {
            get
            {
                return Data.ExtensionIndex;
            }
            set
            {
                Data.ExtensionIndex = value;
            }
        }

        public byte AllocationGroup
        {
            get
            {
                return Data.AllocationGroup;
            }
            set
            {
                Data.AllocationGroup = value;
            }
        }

        public UInt32 CPUSize
        {
            get
            {
                return Data.CPUSize;
            }
            set
            {
                Data.CPUSize = value;
            }
        }

        public UInt32 GPUSize
        {
            get
            {
                return Data.GPUSize;
            }
            set
            {
                Data.GPUSize = value;
            }
        }

        public PrimitiveData Data;

        public Primitive()
        {
            Data = new PrimitiveData();
        }

        public Primitive(Stream stream)
        {
            UInt16 stringLength = stream.ReadUInt16();
            Name = stream.ReadAsciiString(stringLength);

            Data = stream.ReadStruct<PrimitiveData>();
        }

        public void Save(Stream stream)
        {
            stream.WriteUInt16((UInt16)Name.Length);
            stream.WriteAsciiString(Name);
            stream.WriteStruct(Data);
        }
    }
}
