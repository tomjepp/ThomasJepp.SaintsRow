using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC
{
    [StructLayout(LayoutKind.Explicit, Size = 0x08)]
    public struct SoundFXHeader
    {
        [FieldOffset(0x00)]
        public UInt32 ID;

        [FieldOffset(0x04)]
        public UInt32 Unknown;

        [FieldOffset(0x08)]
        public UInt32 IsStreamed;

        [FieldOffset(0x0C)]
        public UInt32 AudioFileId;

        [FieldOffset(0x10)]
        public UInt32 SourceId;
    }

    public class SoundFXObject : IHIRCObject
    {
        public HIRCType Type
        {
            get { return HIRCType.SoundFX; }
        }

        public byte[] Data { get; set; }

        public SoundFXHeader Header { get; set; }

        public UInt32 WemOffset { get; set; }
        public UInt32 WemLength { get; set; }

        public SoundFXObject(byte[] data)
        {
            Data = data;

            using (MemoryStream s = new MemoryStream(data))
            {
                Header = s.ReadStruct<SoundFXHeader>();
                if (Header.IsStreamed != 0)
                {
                    WemOffset = s.ReadUInt32();
                    WemLength = s.ReadUInt32();
                }
            }

            UInt32 temp = Header.AudioFileId % 0x32;

            Console.WriteLine("SoundFX ID: {0:X8} AudioFileID: {1:X8} SourceId: {2:X8} temp {3:X8}", Header.ID, Header.AudioFileId, Header.SourceId, temp);
        }
    }
}
