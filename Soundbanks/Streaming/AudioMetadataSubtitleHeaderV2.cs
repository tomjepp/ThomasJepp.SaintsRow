using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    [StructLayout(LayoutKind.Explicit, Size=0xE4)]
    public struct AudioMetadataSubtitleHeaderV2
    {
        [FieldOffset(0x00)]
        public UInt32 Version;

        [FieldOffset(0x04)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 14)]
        public LocalizedVoiceSubtitleHeader[] MaleLocalizedVoiceSubtitleHeaders;

        [FieldOffset(0x74)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 14)]
        public LocalizedVoiceSubtitleHeader[] FemaleLocalizedVoiceSubtitleHeaders;
    }
}
