using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIV
{
    public class SavegameDirectorySlot
    {
        public byte SlotInUse;

        public uint NumMissionsCompleted;
        public uint NumTakeoversOwned;
        public uint NumHoodsOwned;
        public uint MinutesPlayed;
        public uint DifficultyLevel;
        public bool CheatsEnabled;
        public bool IsAutosave;
        public uint PercentOfGameComplete;
        public uint Version;
        public uint LastQuestCompletedSaveUid;
        public uint CribSaveUid;
        public uint Month;
        public uint Day;
        public uint Year;
        public uint Hours;
        public uint Minutes;
        public uint Seconds;
        public uint Cash;
        public uint RespectLevel;
        public uint NumTakeovers;
        public uint CurrentRespect;

        public SavegameDirectorySlot()
        {

        }

        public SavegameDirectorySlot(Stream s)
        {
            SlotInUse = s.ReadUInt8();

            byte[] data = new byte[24];
            s.Read(data, 0, 24);

            Bitstream bits = new Bitstream(data);
            uint zero = bits.ReadU32(6);
            NumMissionsCompleted = bits.ReadU32(7);
            NumTakeoversOwned = bits.ReadU32(9);
            NumHoodsOwned = bits.ReadU32(5);
            MinutesPlayed = bits.ReadU32(17);
            DifficultyLevel = bits.ReadU32(2);
            CheatsEnabled = bits.ReadBit();
            IsAutosave = bits.ReadBit();
            PercentOfGameComplete = bits.ReadU32(7);
            Version = bits.ReadU32(8);
            LastQuestCompletedSaveUid = bits.ReadU32(7);
            CribSaveUid = bits.ReadU32(5);
            Month = bits.ReadU32(4);
            Day = bits.ReadU32(5);
            Year = bits.ReadU32(7);
            Hours = bits.ReadU32(5);
            Minutes = bits.ReadU32(6);
            Seconds = bits.ReadU32(6);
            Cash = bits.ReadU32(32);
            RespectLevel = bits.ReadU32(6);
            NumTakeovers = bits.ReadU32(9);
            CurrentRespect = bits.ReadU32(32);
        }

        public void Save(Stream s)
        {
            s.WriteUInt8(SlotInUse);

            byte[] data = new byte[24];

            Bitstream bits = new Bitstream(data);
            bits.WriteU32(0, 6);
            bits.WriteU32(NumMissionsCompleted, 7);
            bits.WriteU32(NumTakeoversOwned, 9);
            bits.WriteU32(NumHoodsOwned, 5);
            bits.WriteU32(MinutesPlayed, 17);
            bits.WriteU32(DifficultyLevel, 2);
            bits.WriteBit(CheatsEnabled);
            bits.WriteBit(IsAutosave);
            bits.WriteU32(PercentOfGameComplete, 7);
            bits.WriteU32(Version, 8);
            bits.WriteU32(LastQuestCompletedSaveUid, 7);
            bits.WriteU32(CribSaveUid, 5);
            bits.WriteU32(Month, 4);
            bits.WriteU32(Day, 5);
            bits.WriteU32(Year, 7);
            bits.WriteU32(Hours, 5);
            bits.WriteU32(Minutes, 6);
            bits.WriteU32(Seconds, 6);
            bits.WriteU32(Cash, 32);
            bits.WriteU32(RespectLevel, 6);
            bits.WriteU32(NumTakeovers, 9);
            bits.WriteU32(CurrentRespect, 32);

            s.Write(data, 0, 24);
        }
    }
}
