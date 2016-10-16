using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Fonts
{
    public class FontFile
    {
        public FontHeader Header;
        public List<FontKerningPair> KerningPairs;
        public List<FontCharacter> Characters;

        public List<int> U;
        public List<int> V;

        public FontFile(Stream s)
        {
            Header = s.ReadStruct<FontHeader>();

            if (Header.Version != 4)
                throw new Exception("Unknown font version: " + Header.Version.ToString());

            KerningPairs = new List<FontKerningPair>();

            for (int i = 0; i < Header.NumberOfKerningPairs; i++)
            {
                FontKerningPair kerningPair = s.ReadStruct<FontKerningPair>();
                KerningPairs.Add(kerningPair);
            }

            Characters = new List<FontCharacter>();

            for (int i = 0; i < Header.NumberOfCharacters; i++)
            {
                FontCharacter character = s.ReadStruct<FontCharacter>();
                Characters.Add(character);
            }

            U = new List<int>();
            for (int i = 0; i < Header.NumberOfCharacters; i++)
            {
                int u = s.ReadInt32();
                U.Add(u);
            }

            V = new List<int>();
            for (int i = 0; i < Header.NumberOfCharacters; i++)
            {
                int v = s.ReadInt32();
                V.Add(v);
            }
        }
    }
}
