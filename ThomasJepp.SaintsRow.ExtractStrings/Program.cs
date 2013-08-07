using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using CmdLine;
using ThomasJepp.SaintsRow.Strings;

namespace ThomasJepp.SaintsRow.ExtractStrings
{
    public static class Program
    {
        [CommandLineArguments(Program = "ThomasJepp.SaintsRow.ExtractStrings", Title = "Saints Row Localisation File Extractor", Description = "Extracts Saints Row PC Localisation files. Supports Saints Row 2, Saints Row: The Third and Saints Row IV.")]
        internal class Options
        {
            [CommandLineParameter(Command = "sr2", Default = false, Description = "Extract a Saints Row 2 file. If not specified, the format used in Saints Row: The Third and Saints Row IV is assumed.", Name = "Saints Row 2 Mode")]
            public bool SaintsRow2Mode { get; set; }

            [CommandLineParameter(Command = "xtbl", Description = "The path of a folder containing the XTBL files from the game matching the specified input file. This is used to provide more understandable output with the correct language string names. If not specified language strings will be identified by their hash.", Name = "Location of XTBL files.", ValueExample=@"path")]
            public string XtblPath { get; set; }

            [CommandLineParameter(Name = "input", ParameterIndex = 1, Required = true, Description = "The localisation file to extract.")]
            public string Input { get; set; }

            [CommandLineParameter(Name = "output", ParameterIndex = 2, Required = false, Description = "The output file to create. If not specified, the input filename will be used with the exension changed to \".txt\".")]
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

            Dictionary<UInt32, string> hashLookup = new Dictionary<UInt32, string>();
            if (options.XtblPath != null)
            {
                Console.WriteLine("Loading XTBL files...");
                string[] xtblFiles = Directory.GetFiles(options.XtblPath, "*.xtbl");
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

            string outputFile = (options.Output != null) ? options.Output : Path.ChangeExtension(options.Input, ".txt");

            Console.WriteLine("Extracting {0} to {1}...", options.Input, outputFile);

            StringFile stringFile = null;

            using (Stream stream = File.OpenRead(options.Input))
            {
                 stringFile = new StringFile(stream, options.SaintsRow2Mode);
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
