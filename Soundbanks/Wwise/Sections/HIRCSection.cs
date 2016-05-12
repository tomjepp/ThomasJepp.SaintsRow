using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections
{
    public class HIRCSection : ISoundbankSection
    {
        public SectionId SectionId { get { return Wwise.SectionId.HIRC; } }

        public byte[] Data { get; set; }

        List<IHIRCObject> Objects = new List<IHIRCObject>();

        public HIRCSection(byte[] data)
        {
            Data = data;

            using (MemoryStream s = new MemoryStream(data))
            {
                UInt32 objectCount = s.ReadUInt32();
                for (int i = 0; i < objectCount; i++)
                {
                    Console.Write("HIRC object at: {0:X}", s.Position);
                    HIRCType type = (HIRCType)s.ReadUInt8();
                    UInt32 length = s.ReadUInt32();
                    byte[] buffer = new byte[length];
                    s.Read(buffer, 0, (int)length);

                    Console.WriteLine(" Type {0} Length {1:X}", type, length);

                    IHIRCObject obj = HIRCObject.GetObject(type, buffer);
                    Objects.Add(obj);
                }
            }
        }
    }
}
