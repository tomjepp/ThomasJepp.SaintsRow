using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod
{
    public class Section
    {
        private SaveGameSectionHeader Header;

        public SectionId SectionId
        {
            get
            {
                return Header.SectionId;
            }
            set
            {
                Header.SectionId = value;
            }
        }

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

        public UInt32 Size
        {
            get
            {
                return Header.Size;
            }
            set
            {
                Header.Size = value;
            }
        }

        private byte[] _Data;
        public byte[] Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                Size = (uint)_Data.Length;
            }
        }

        public Section(Stream s)
        {
            Header = s.ReadStruct<SaveGameSectionHeader>();
            Data = new byte[Header.Size];
            s.Read(Data, 0, (int)Header.Size);
        }

        public void Save(Stream s)
        {
            s.WriteStruct<SaveGameSectionHeader>(Header);
            s.Write(Data, 0, Data.Length);
        }
    }
}
