using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections.BKHD;

namespace ThomasJepp.SaintsRow.Soundbanks.Wwise.Sections
{
    public class BKHDSection : ISoundbankSection
    {
        public SectionId SectionId { get { return Wwise.SectionId.BKHD; } }

        public byte[] Data { get; set; }

        public UInt32 Version
        {
            get
            {
                return Header.Version;
            }
            set
            {
                Header.Version = value;
            }
        }

        public UInt32 SoundbankID
        {
            get
            {
                return Header.ID;
            }
            set
            {
                Header.ID = value;
            }
        }

        private BKHDHeader Header;

        public BKHDSection(byte[] data)
        {
            Data = data;
            Header = data.ReadStruct<BKHDHeader>(0);
        }
    }
}
