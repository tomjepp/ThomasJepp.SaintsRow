using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    /* 00000000 localized_voice_subtitle_header struc ; (sizeof=0x8, align=0x4, standard type)
     * 00000000                                         ; XREF: audio_metadata_subtitle_header
     * 00000000 m_length        dd ?
     * 00000004 m_offset        dd ?
     * 00000008 localized_voice_subtitle_header ends
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x08)]
    public struct LocalizedVoiceSubtitleHeader
    {
        [FieldOffset(0x00)]
        public UInt32 Length;

        [FieldOffset(0x04)]
        public UInt32 Offset;
    }
}
