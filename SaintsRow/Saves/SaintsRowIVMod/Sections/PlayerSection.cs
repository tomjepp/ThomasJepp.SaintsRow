using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Saves.SaintsRowIVMod;
using ThomasJepp.SaintsRow.Saves.SaintsRowIVMod.Sections.Player;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod.Sections
{
    public class PlayerSection
    {
        private Section _Section;
        private SavedPlayerData _SavedPlayerData;

        public PlayerSection(Section section)
        {
            _Section = section;
            _SavedPlayerData = _Section.Data.ReadStruct<SavedPlayerData>(0);
        }

        public decimal CashOnHand
        {
            get
            {
                return (decimal)_SavedPlayerData.CashOnHand / 100m;
            }
            set
            {
                decimal cents = value * 100m;
                _SavedPlayerData.CashOnHand = (Int32)cents;
                _Section.Data.WriteStruct(_SavedPlayerData, 0);
            }
        }

        public Int32 Orbs
        {
            get
            {
                return _SavedPlayerData.Orbs;
            }
            set
            {
                _SavedPlayerData.Orbs = value;
                _Section.Data.WriteStruct(_SavedPlayerData, 0);
            }
        }
    }
}
