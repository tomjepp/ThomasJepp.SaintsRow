using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    /*
     * uint32 file id
     * uint32 offset from top of file
     * uint32 metadata length(subtitle, etc)
     * uint32 length
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct SoundbankEntryInfo
    {
        [FieldOffset(0x00)]
        public UInt32 FileId;

        [FieldOffset(0x04)]
        public UInt32 Offset;

        [FieldOffset(0x08)]
        public UInt32 MetadataLength;

        [FieldOffset(0x0C)]
        public UInt32 AudioLength;
    }
}
