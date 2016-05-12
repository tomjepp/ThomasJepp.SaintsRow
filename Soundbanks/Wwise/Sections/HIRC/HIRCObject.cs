using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC
{
    public static class HIRCObject
    {
        public static IHIRCObject GetObject(HIRCType type, byte[] data)
        {
            switch (type)
            {
                case HIRCType.ActorMixer:
                case HIRCType.Attenuation:
                case HIRCType.AudioBus:
                case HIRCType.AuxiliaryBus:
                case HIRCType.Effect:
                case HIRCType.Settings:
                case HIRCType.SwitchContainer:
                case HIRCType.Unknown:
                    return new GenericObject(type, data);

                case HIRCType.SoundFX:
                    return new SoundFXObject(data);

                default:
                    throw new NotImplementedException(type.ToString());
            }
        }
    }
}
