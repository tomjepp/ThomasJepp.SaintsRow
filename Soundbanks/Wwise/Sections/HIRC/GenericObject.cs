using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC
{
    public class GenericObject : IHIRCObject
    {
        public HIRCType Type { get; private set; }

        public byte[] Data { get; set; }

        public GenericObject(HIRCType type, byte[] data)
        {
            Type = type;
            Data = data;
            Console.WriteLine("Generic Object: {0} length: {1:X}", type, data);
        }
    }
}
