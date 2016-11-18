using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIV
{
    public class SavegameDirectory
    {
        public uint SlotsDataChecksum;
        public uint SlotsUsed;
        public SavegameDirectorySlot[] Slots = new SavegameDirectorySlot[24];
        
        public SavegameDirectory()
        {
            for (int i = 0; i < 24; i++)
            {
                Slots[i] = new SavegameDirectorySlot();
            }
        }

        public SavegameDirectory(Stream s)
        {
            SlotsDataChecksum = s.ReadUInt32();
            SlotsUsed = s.ReadUInt32();

            for (int i = 0; i < 24; i++)
            {
                SavegameDirectorySlot slot = new SavegameDirectorySlot(s);
            }
        }

        public void Save(Stream s)
        {
            byte[] slotData = null;

            using (MemoryStream ms = new MemoryStream())
            {
                for (int i = 0; i < 24; i++)
                {
                    Slots[i].Save(ms);
                }

                ms.Seek(0, SeekOrigin.Begin);
                slotData = ms.ToArray();
            }

            SlotsDataChecksum = Hashes.CrcVolition(slotData);
            s.WriteUInt32(SlotsDataChecksum);
            s.WriteUInt32(SlotsUsed);
            s.Write(slotData, 0, 600);
        }
    }
}
