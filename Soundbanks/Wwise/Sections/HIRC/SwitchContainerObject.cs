using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.HIRC
{
    public class SwitchContainerObject : IHIRCObject
    {
        public HIRCType Type
        {
            get { return HIRCType.SwitchContainer; }
        }

        public byte[] Data { get; set; }

        public UInt32 ID { get; set; }

        public uint OutputBus { get; set; }
        public uint ParentObjectId { get; set; }

        public SwitchContainerObject(byte[] data)
        {
            Data = data;

            using (MemoryStream s = new MemoryStream(data))
            {
                ID = s.ReadUInt32();
                bool overrideEffects = s.ReadBoolean();
                byte effectCount = s.ReadUInt8();
                if (effectCount > 0)
                {
                    s.ReadUInt8(); // effectmask
                }
                for (int i = 0; i < effectCount; i++)
                {
                    s.ReadUInt8(); // effect index
                    s.ReadUInt32(); // effect id
                    s.ReadUInt16(); // zeroes
                }
                OutputBus = s.ReadUInt32();
                ParentObjectId = s.ReadUInt32();
                s.ReadBoolean(); // overrideParentPlaybackPriority
                s.ReadBoolean(); // offset priority by ... at max distance activated
                
                byte additionalParameterCount = s.ReadUInt8();
                byte[] additionalParameterTypes = new byte[additionalParameterCount];
                for (int i = 0; i < additionalParameterCount; i++)
                {
                    additionalParameterTypes[i] = s.ReadUInt8();
                }
                for (int i = 0; i < additionalParameterCount; i++)
                {
                    s.ReadUInt32(); // most of these are floats but it doesn't actually matter. 4 bytes is 4 bytes
                }

                s.ReadUInt8(); // unknown

                bool positioningSection = s.ReadBoolean();
                if (positioningSection)
                {
                    byte positionType = s.ReadUInt8();
                    if (positionType == 0x00)
                    {
                        // 2D
                        s.ReadBoolean(); // panner enabled?
                    }
                    else if (positionType == 0x01)
                    {
                        // 3D
                        uint positionSource = s.ReadUInt32();
                        s.ReadUInt32(); // attenuation object
                        s.ReadBoolean(); // spatialization?
                        if (positionSource == 0x02)
                        {
                            // User defined
                            s.ReadUInt32(); // play type
                            s.ReadBoolean(); // loop? 
                            s.ReadUInt32(); // transition time
                            s.ReadBoolean(); // follow listener orientation?
                        }
                        else if (positionSource == 0x03)
                        {
                            // Game defined
                            s.ReadBoolean(); // update at each frame?
                        }
                    }
                }

                s.ReadBoolean(); // override parent settings for Game-Defined Auxiliary Sends?
                s.ReadBoolean(); // use Game-Defined Auxiliary Sends?
                s.ReadBoolean(); // override parent settings for User-Defined Auxiliary Sends?
                bool udasExist = s.ReadBoolean(); // User-Defined Auxiliary Sends exist?
                if (udasExist)
                {
                    uint auxBus0 = s.ReadUInt32(); // Auxiliary bus 0
                    uint auxBus1 = s.ReadUInt32(); // Auxiliary bus 1
                    uint auxBus2 = s.ReadUInt32(); // Auxiliary bus 2
                    uint auxBus3 = s.ReadUInt32(); // Auxiliary bus 3
                }
                
                bool unkPlaybackLimit = s.ReadBoolean(); // unknown param for playback limit
                if (unkPlaybackLimit)
                {
                    s.ReadUInt8(); // priority equal action
                    s.ReadUInt8(); // limit action
                    s.ReadUInt16(); // limit sound instances to ...
                }

                s.ReadUInt8(); // how to limit source instances
                s.ReadUInt8(); // virtual voice behaviour

                s.ReadBoolean(); // override parent settings for playback limit
                s.ReadBoolean(); // override parent settings for virtual voice

                uint stateGroupCount = s.ReadUInt32();
                for (int i = 0; i < stateGroupCount; i++)
                {
                    uint stateGroupId = s.ReadUInt32();
                    s.ReadUInt8(); // change occurs at
                    ushort statesDifferentFromDefault = s.ReadUInt16();
                    for (int j = 0; j < statesDifferentFromDefault; j++)
                    {
                        uint stateObjectId = s.ReadUInt32();
                        uint objectWithSettings = s.ReadUInt32();
                        Console.WriteLine("{0:X8} {1:X8} {2:X8}", stateGroupId, stateObjectId, objectWithSettings);
                    }
                }
            }


            Console.WriteLine("SwitchContainer ID: {0:X8} OutputBus: {1:X8} ParentObjectId: {2:X8}", ID, OutputBus, ParentObjectId);
        }
    }
}
