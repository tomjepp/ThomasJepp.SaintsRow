using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    /*
     * 00000000 audio_metadata_subtitle_header struc ; (sizeof=0x74, align=0x4)
     * 00000000 m_version       dd ?
     * 00000004 m_localized_subtitle_header localized_voice_subtitle_header 14 dup(?)
     * 00000074 audio_metadata_subtitle_header ends
     */

    [StructLayout(LayoutKind.Explicit, Size=0x74)]
    public struct AudioMetadataSubtitleHeaderV3
    {
        [FieldOffset(0x00)]
        public UInt32 Version;

        [FieldOffset(0x04)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 14)]
        public LocalizedVoiceSubtitleHeader[] LocalizedVoiceSubtitleHeaders;
    }
}
