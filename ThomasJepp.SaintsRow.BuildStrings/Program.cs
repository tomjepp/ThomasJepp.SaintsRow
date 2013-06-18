using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using ThomasJepp.SaintsRow.Strings;

namespace ThomasJepp.SaintsRow.BuildStrings
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:\nThomasJepp.SaintsRow.BuildStrings.exe [-sr2] <text file>");
                return;
            }

            bool sr2Mode = false;

            for (int i = 0; i < args.Length - 1; i++)
            {
                string arg = args[i];
                if (arg.ToLowerInvariant() == "-sr2")
                    sr2Mode = true;
            }

            string file = args[args.Length - 1];
            string outputFile = Path.ChangeExtension(file, ".le_strings");

            Console.WriteLine("Packing {0} and creating {1}...", file, outputFile);

            string[] lines = File.ReadAllLines(file);

            UInt16 bucketCount = (UInt16)(lines.Length / 5); // work this out properly
            if (bucketCount < 32)
                bucketCount = 32;
            else if (bucketCount < 64)
                bucketCount = 64;
            else if (bucketCount < 128)
                bucketCount = 128;
            else if (bucketCount < 256)
                bucketCount = 256;
            else if (bucketCount < 512)
                bucketCount = 512;
            else 
                bucketCount = 1024;

            StringFile stringFile = new StringFile(bucketCount, sr2Mode);

            foreach (string line in lines)
            {
                string[] pieces = line.Split(new char[] { ':' }, 2);
                string key = pieces[0];
                UInt32 hash = 0;
                string value = pieces[1].Trim(' ', '"').Replace("\\n", "\n");
                if (key.StartsWith("\"") && key.EndsWith("\""))
                    hash = Hashes.CrcVolition(key.Trim('"'));
                else
                    hash = UInt32.Parse(key);

                stringFile.AddString(hash, value);
            }

            using (Stream s = File.OpenWrite(outputFile))
            {
                stringFile.Save(s);
            }

            Console.WriteLine("Done.");
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
