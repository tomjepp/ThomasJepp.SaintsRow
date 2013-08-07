using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using CmdLine;
using ThomasJepp.SaintsRow.Strings;

namespace ThomasJepp.SaintsRow.BuildStrings
{
    public static class Program
    {
        [CommandLineArguments(Program = "ThomasJepp.SaintsRow.BuildStrings", Title = "Saints Row Localisation File Builder", Description = "Builds Saints Row PC Localisation files. Supports Saints Row 2, Saints Row: The Third and Saints Row IV.")]
        internal class Options
        {
            [CommandLineParameter(Command = "sr2", Default = false, Description = "Build files suitable for Saints Row 2. If not specified, files suitable for Saints Row: The Third and Saints Row IV will be built.", Name = "Saints Row 2 Mode")]
            public bool SaintsRow2Mode { get; set; }

            [CommandLineParameter(Name = "input", ParameterIndex = 1, Required = true, Description = "The file containing the language strings to use.")]
            public string Input { get; set; }

            [CommandLineParameter(Name = "output", ParameterIndex = 2, Required = false, Default = "output", Description = "The output file to create. If not specified, the input filename will be used with the extension changed to \".le_strings\".")]
            public string Output { get; set; }
        }

        public static void Main(string[] args)
        {
            Options options = null;

            try
            {
                options = CommandLine.Parse<Options>();
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine();
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));

#if DEBUG
                Console.ReadLine();
#endif
                return;
            }

            string outputFile = options.Output != null ? options.Output : Path.ChangeExtension(options.Input, ".le_strings");

            Console.WriteLine("Packing {0} and creating {1}...", options.Input, outputFile);

            string[] lines = File.ReadAllLines(options.Input);

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

            StringFile stringFile = new StringFile(bucketCount, options.SaintsRow2Mode);

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

                if (stringFile.ContainsKey(hash))
                {
                    Console.WriteLine("You are attempting to add a duplicate key to the strings file.");
                    Console.WriteLine("Key: \"{0}\", Hash: {1}, Value: {2}", key, hash, value);
                    Console.WriteLine("Other value: {0}", stringFile.GetString(hash));
                    return;
                }
                
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
