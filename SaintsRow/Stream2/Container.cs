﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Stream2
{
    public class Container
    {
        public string Name { get; set; }
        public byte ContainerType { get; set; }
        public ContainerFlags Flags { get; set; }
        public UInt16 PrimitiveCount { get; set; }
        public UInt32 PackfileBaseOffset { get; set; } // Data start offset in str2
        public byte CompressionType { get; set; } // Always 0x09?
        public string StubContainerParentName { get; set; }
        public byte[] AuxData { get; set; }
        public Int32 TotalCompressedPackfileReadSize { get; set; } // Size of condensed data

        public List<WriteTimeSizes> PrimitiveSizes { get; set; }

        public List<Primitive> Primitives { get; set; }

        public Container(Stream stream)
        {
            UInt16 stringLength = stream.ReadUInt16();
            Name = stream.ReadAsciiString(stringLength);
            ContainerType = stream.ReadUInt8();
            Flags = (ContainerFlags)stream.ReadUInt16();
            PrimitiveCount = stream.ReadUInt16();
            PackfileBaseOffset = stream.ReadUInt32();
            CompressionType = stream.ReadUInt8();
            stringLength = stream.ReadUInt16();
            StubContainerParentName = stream.ReadAsciiString(stringLength);
            Int32 auxDataSize = stream.ReadInt32();
            AuxData = new byte[auxDataSize];
            stream.Read(AuxData, 0, auxDataSize);
            TotalCompressedPackfileReadSize = stream.ReadInt32();

            Primitives = new List<Primitive>();
            PrimitiveSizes = new List<WriteTimeSizes>();

            for (UInt16 i = 0; i < PrimitiveCount; i++)
            {
                var sizes = stream.ReadStruct<WriteTimeSizes>();
                PrimitiveSizes.Add(sizes);
            }

            for (UInt16 i = 0; i < PrimitiveCount; i++)
            {
                Primitive primitive = new Primitive(stream);
                Primitives.Add(primitive);
            }
        }

        public void Save(Stream stream)
        {
            stream.WriteUInt16((UInt16)Name.Length);
            stream.WriteAsciiString(Name);
            stream.WriteUInt8(ContainerType);
            stream.WriteUInt16((UInt16)Flags);
            stream.WriteUInt16((UInt16)Primitives.Count);
            stream.WriteUInt32(PackfileBaseOffset);
            stream.WriteUInt8(CompressionType);
            if (StubContainerParentName != null && StubContainerParentName != "")
            {
                stream.WriteUInt16((UInt16)StubContainerParentName.Length);
                stream.WriteAsciiString(StubContainerParentName);
            }
            else
            {
                stream.WriteUInt16(0);
            }
            stream.WriteInt32(AuxData.Length);
            stream.Write(AuxData, 0, AuxData.Length);
            stream.WriteInt32(TotalCompressedPackfileReadSize);

            foreach (WriteTimeSizes sizes in PrimitiveSizes)
            {
                stream.WriteStruct(sizes);
            }

            foreach (Primitive primitive in Primitives)
            {
                primitive.Save(stream);
            }
        }
    }
}
