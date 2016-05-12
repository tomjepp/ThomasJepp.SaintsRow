using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.GameInstances;

namespace ThomasJepp.SaintsRow.Localization
{
    public static class LanguageUtility
    {
        public static string GetLanguageCode(Language language)
        {
            switch (language)
            {
                case Language.English: return "US";
                case Language.Spanish: return "ES";
                case Language.Italian: return "IT";
                case Language.Japanese: return "JP";
                case Language.German: return "DE";
                case Language.French: return "FR";
                case Language.Dutch: return "NL";
                case Language.Swedish: return "SE";
                case Language.Danish: return "DK";
                case Language.Czech: return "CZ";
                case Language.Polish: return "PL";
                case Language.Korean: return "SK";
                case Language.Russian: return "RU";
                case Language.Chinese: return "CH";
                default: throw new NotImplementedException();
            }
        }

        public static Language GetLanguageFromCode(string code)
        {
            switch (code.ToUpperInvariant())
            {
                case "US": return Language.English;
                case "ES": return Language.Spanish;
                case "IT": return Language.Italian;
                case "JP": return Language.Japanese;
                case "DE": return Language.German;
                case "FR": return Language.French;
                case "NL": return Language.Dutch;
                case "SE": return Language.Swedish;
                case "DK": return Language.Danish;
                case "CZ": return Language.Czech;
                case "PL": return Language.Polish;
                case "SK": return Language.Korean;
                case "RU": return Language.Russian;
                case "CH": return Language.Chinese;
                default:
                    {
                        Language lang;
                        if (Enum.TryParse<Language>(code, out lang))
                            return lang;
                        else
                            throw new NotImplementedException();
                    }
            }
        }

        public static Dictionary<char, char> GetDecodeCharMap(IGameInstance instance, Language language)
        {
            string filename = null;
            string packfile = null;
            if (instance.Game == GameSteamID.SaintsRow2)
            {
                filename = String.Format("charlist_{0}.txt", GetLanguageCode(language).ToLowerInvariant());
                packfile = "patch.vpp_pc";
            }
            else
            {
                filename = String.Format("charlist_{0}.dat", GetLanguageCode(language).ToLowerInvariant());
                packfile = "misc.vpp_pc";
            }

            using (Stream stream = instance.OpenPackfileFile(filename, packfile))
            {
                return GetDecodeCharMapInternal(stream, language);
            }
        }

        private static Dictionary<char, char> GetDecodeCharMapInternal(Stream charmapStream, Language language)
        {
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(charmapStream))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.StartsWith("//"))
                        continue;

                    if (line.StartsWith("count="))
                        continue;

                    lines.Add(line);
                }
            }

            Dictionary<char, char> map = new Dictionary<char, char>();

            int nextSlot = 0x100;
            foreach (string line in lines)
            {
                int value = 0;
                if (int.TryParse(line, out value))
                {
                    if (value > 0x100)
                    {
                        map.Add((char)nextSlot, (char)value);
                        nextSlot++;
                    }
                    else
                    {
                        map.Add((char)value, (char)value);
                    }
                }
            }

            return map;
        }

        public static Dictionary<char, char> GetEncodeCharMap(IGameInstance instance, Language language)
        {
            string filename = null;
            string packfile = null;
            if (instance.Game == GameSteamID.SaintsRow2)
            {
                filename = String.Format("charlist_{0}.txt", GetLanguageCode(language).ToLowerInvariant());
                packfile = "patch.vpp_pc";
            }
            else
            {
                filename = String.Format("charlist_{0}.dat", GetLanguageCode(language).ToLowerInvariant());
                packfile = "misc.vpp_pc";
            }

            using (Stream stream = instance.OpenPackfileFile(filename, packfile))
            {
                return GetEncodeCharMapInternal(stream, language);
            }
        }

        private static Dictionary<char, char> GetEncodeCharMapInternal(Stream charmapStream, Language language)
        {
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(charmapStream))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.StartsWith("//"))
                        continue;

                    if (line.StartsWith("count="))
                        continue;

                    lines.Add(line);
                }
            }

            Dictionary<char, char> map = new Dictionary<char, char>();

            int nextSlot = 0x100;
            foreach (string line in lines)
            {
                int value = 0;
                if (int.TryParse(line, out value))
                {
                    if (value > 0x100)
                    {
                        map.Add((char)value, (char)nextSlot);
                        nextSlot++;
                    }
                    else
                    {
                        map.Add((char)value, (char)value);
                    }
                }
            }

            return map;
        }
    }
}
