using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using ThomasJepp.SaintsRow.Strings;

namespace ThomasJepp.SaintsRow.ExtractStrings
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:\nThomasJepp.SaintsRow.ExtractStrings.exe [-sr2] [-xtbl=path] <string file>");
                return;
            }

            bool sr2Mode = false;
            string xtblPath = null;

            for (int i = 0; i < args.Length - 1; i++)
            {
                string arg = args[i];
                if (arg.ToLowerInvariant() == "-sr2")
                    sr2Mode = true;
                if (arg.ToLowerInvariant().StartsWith("-xtbl="))
                    xtblPath = arg.Remove(0, 6);
            }

            Dictionary<UInt32, string> hashLookup = new Dictionary<UInt32, string>();
            if (xtblPath != null)
            {
                Console.WriteLine("Loading XTBL files...");
                string[] xtblFiles = Directory.GetFiles(xtblPath, "*.xtbl");
                foreach (string xtblFile in xtblFiles)
                {
                    StreamReader reader = new StreamReader(xtblFile);
                    string xtbl = reader.ReadToEnd();
                    reader.Close();
                    Regex regex = new Regex("<Name>(.*?)</Name>", RegexOptions.Compiled);
                    foreach (Match m in regex.Matches(xtbl))
                    {
                        uint hash = Hashes.CrcVolition(m.Groups[1].Value);
                        if (!hashLookup.ContainsKey(hash))
                            hashLookup.Add(hash, m.Groups[1].Value);
                    }
                }
            }

            string file = args[args.Length-1];
            string outputFile = Path.ChangeExtension(file, ".txt");

            Console.WriteLine("Extracting {0} to {1}...", file, outputFile);

            StringFile stringFile = null;

            using (Stream stream = File.OpenRead(file))
            {
                 stringFile = new StringFile(stream, sr2Mode);
            }

            var hashes = stringFile.GetHashes();

            Dictionary<string, string> hashStrings = new Dictionary<string, string>();
            foreach (var hash in hashes)
            {
                string text = stringFile.GetString(hash).Replace("\n", "\\n");
                if (hashLookup.ContainsKey(hash))
                {
                    hashStrings.Add("\"" + hashLookup[hash] + "\"", text);
                }
                else
                {
                    hashStrings.Add(String.Format("{0}", hash), text);
                }
            }

            string[] keys = hashStrings.Keys.ToArray();
            Array.Sort(keys);

            using (StreamWriter sw = new StreamWriter(outputFile, false, System.Text.Encoding.UTF8))
            {
                foreach (string key in keys)
                {
                    string text = hashStrings[key];
                    sw.WriteLine("{0}: \"{1}\"", key, text);
                }
            }

            Console.WriteLine("Done.");
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
