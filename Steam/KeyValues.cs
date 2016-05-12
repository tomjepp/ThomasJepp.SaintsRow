using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Steam
{
    public class KeyValues
    {
        public Dictionary<string, object> Items = new Dictionary<string,object>(StringComparer.InvariantCultureIgnoreCase);
        public KeyValues(Stream s)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                while (!sr.EndOfStream)
                {
                    object keyToken = ReadToken(sr);
                    if (keyToken == null)
                    {
                        EatWhitespace(sr);
                        continue;
                    }

                    string key = (string)keyToken;
                    object value = ReadToken(sr);

                    Items.Add(key, value);

                    EatWhitespace(sr);
                }
            }
        }

        private void EatWhitespace(StreamReader sr)
        {
            while (char.IsWhiteSpace(sr.PeekChar()))
            {
                sr.ReadChar();
            }
        }

        private object ReadToken(StreamReader sr)
        {
            EatWhitespace(sr);

            char c = sr.ReadChar();
            switch (c)
            {
                case '{':
                    return ReadDict(sr);
                case '/':
                    {
                        if (sr.PeekChar() == '/')
                        {
                            sr.ReadLine();
                            return null;
                        }
                        else
                        {
                            return ReadString(sr, c);
                        }
                    }

                default:
                    return ReadString(sr, c);
            }
        }

        private Dictionary<string, object> ReadDict(StreamReader sr)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            EatWhitespace(sr);
            while (sr.PeekChar() != '}')
            {
                EatWhitespace(sr);
                object keyToken = ReadToken(sr);
                if (keyToken != null)
                {
                    string key = (string)keyToken;
                    object value = ReadToken(sr);
                    dict.Add(key, value);
                }
                EatWhitespace(sr);
            }
            sr.ReadChar(); // eat the }

            return dict;
        }

        private string ReadString(StreamReader sr)
        {
            return ReadString(sr, sr.ReadChar());
        }

        private string ReadString(StreamReader sr, char firstChar)
        {
            StringBuilder sb = new StringBuilder();

            bool readToQuote = false;

            if (firstChar == '"')
            {
                readToQuote = true;
            }
            else
            {
                sb.Append(firstChar);
            }

            while (true)
            {
                char c = sr.ReadChar();

                if (char.IsWhiteSpace(c) && !readToQuote)
                {
                    return sb.ToString();
                }
                else if (c == '"' && readToQuote)
                {
                    return sb.ToString();
                }
                else if (c == '\\')
                {
                    char escaped = sr.ReadChar();
                    switch (escaped)
                    {
                        case 'n':
                            sb.Append((char)0x0A);
                            break;
                        case 'r':
                            sb.Append((char)0x0D);
                            break;
                        default:
                            sb.Append(escaped);
                            break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
        }
    }
}
