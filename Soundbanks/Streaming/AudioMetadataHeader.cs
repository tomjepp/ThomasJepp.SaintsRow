using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Streaming
{
    /*
     * 00000000
     * 00000000 audio_metadata_header struc ; (sizeof=0x24, align=0x4)
     * 00000000 signature       dd ?
     * 00000004 version         dd ?
     * 00000008 persona_id      dd ?
     * 0000000C voiceline_id    dd ?
     * 00000010 lipsync_size    dd ?
     * 00000014 lipsync_offset  dd ?
     * 00000018 subtitle_size   dd ?
     * 0000001C subtitle_offset dd ?
     * 00000020 wav_length_ms   dd ?
     * 00000024 audio_metadata_header ends
     * 00000024
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x24)]
    public struct AudioMetadataHeader
    {
        [FieldOffset(0x00)]
        public UInt32 Signature;

        [FieldOffset(0x04)]
        public UInt32 Version;

        [FieldOffset(0x08)]
        public UInt32 PersonaID;

        [FieldOffset(0x0C)]
        public UInt32 VoicelineID;

        [FieldOffset(0x10)]
        public UInt32 LipsyncSize;

        [FieldOffset(0x14)]
        public UInt32 LipsyncOffset;

        [FieldOffset(0x18)]
        public UInt32 SubtitleSize;

        [FieldOffset(0x1C)]
        public UInt32 SubtitleOffset;

        [FieldOffset(0x20)]
        public UInt32 WavLengthMs;
    }
}
