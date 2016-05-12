using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    /*
     * uint32 signature (VWSB)
     * uint32 platform
     * uint32 timestamp(lower 32 bits)
     * uint16 version
     * uint16 cruncher version
     * uint32 wwise bank id
     * uint32 header size(includes all file data)
     * uint16 num files
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x1C)]
    public struct SoundbankHeader
    {
        [FieldOffset(0x00)]
        public UInt32 Signature;

        [FieldOffset(0x04)]
        public UInt32 Platform;

        [FieldOffset(0x08)]
        public UInt32 Timestamp;

        [FieldOffset(0x0C)]
        public UInt16 Version;

        [FieldOffset(0x0E)]
        public UInt16 CruncherVersion;

        [FieldOffset(0x10)]
        public UInt32 WwiseBankId;

        [FieldOffset(0x14)]
        public UInt32 HeaderSize;

        [FieldOffset(0x18)]
        public UInt32 NumFiles;
    }
}
