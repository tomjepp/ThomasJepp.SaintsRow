using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Stream2
{
    public class Primitive
    {
        public string Name { get; set; }
        public PrimitiveData Data;

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
