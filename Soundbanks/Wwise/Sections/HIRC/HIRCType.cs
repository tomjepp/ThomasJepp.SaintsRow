using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC
{
    public enum HIRCType : byte
    {
        Settings = 1,
        SoundFX = 2,
        EventAction = 3,
        Event = 4,
        SequenceContainer = 5,
        SwitchContainer = 6,
        ActorMixer = 7,
        AudioBus = 8,
        BlendContainer = 9,
        MusicSegment = 10,
        MusicTrack = 11,
        MusicSwitchContainer = 12,
        MusicPlaylistContainer = 13,
        Attenuation = 14,
        DialogueEvent = 15,
        MotionBus = 16,
        MotionFX = 17,
        Effect = 18,
        Unknown = 19,
        AuxiliaryBus = 20
    }
}
