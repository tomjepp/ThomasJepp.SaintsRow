using System;
using System.Collections.Generic;
using System.IO;

using CmdLine;
using ThomasJepp.SaintsRow.Packfiles;

namespace ThomasJepp.SaintsRow.RecursiveExtractor
{
    class Program
    {
        [CommandLineArguments(Program = "ThomasJepp.SaintsRow.RecursiveExtractor", Title = "Saints Row Recursive Packfile Extractor", Description = "Extracts all Saints Row PC packfiles in the specified folder and any packfiles that they contain. Supports Saints Row IV.")]
        internal class Options
        {
            [CommandLineParameter(Name = "source", ParameterIndex = 1, Required = true, Description = "The folder containing packfiles to extract.")]
            public string Source { get; set; }

            [CommandLineParameter(Name = "output", ParameterIndex = 2, Required = false, Default="output", Description = "The folder to extract the packfiles to. This will be created if it does not already exist. If not specified, a folder named \"output\" will be created in the current directory.")]
            public string Output { get; set; }
       }

        static void Main(string[] args)
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

            Directory.CreateDirectory(options.Output);

            string[] files = Directory.GetFiles(options.Source);

            Dictionary<string, IPackfile> packfiles = new Dictionary<string, IPackfile>();

            int totalFiles = 0;

            Console.Write("Looking for packfiles... ");

            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".vpp_pc")
                {
                    var packfile = Packfile.FromStream(File.OpenRead(file));
                    totalFiles += packfile.Files.Count;
                    packfiles.Add(Path.GetFileName(file), packfile);
                }
            }

            Console.WriteLine("found {0} packfiles, containing {1} files.", packfiles, totalFiles);

            int currentFile = 0;

            foreach (var packfilePair in packfiles)
            {
                Directory.CreateDirectory(Path.Combine(options.Output, packfilePair.Key));
                foreach (var file in packfilePair.Value.Files)
                {
                    currentFile++;
                    if (Path.GetExtension(file.Name) == ".str2_pc")
                    {
                        string strOutputFolder = Path.Combine(options.Output, packfilePair.Key, file.Name);
                        Directory.CreateDirectory(strOutputFolder);
                        Console.WriteLine("[{0}/{1}] Extracting {2}: packfile {3} to {4}:", currentFile, totalFiles, packfilePair.Key, file.Name, strOutputFolder);
                        using (Stream strStream = file.GetStream())
                        {
                            using (var strPackfile = Packfile.FromStream(strStream))
                            {
                                int strCurrentFile = 0;

                                foreach (var strFile in strPackfile.Files)
                                {
                                    strCurrentFile++;

                                    Console.Write("[{0}/{1}] [{2}/{3}] Extracting {4}\\{5}: {6}", currentFile, totalFiles, strCurrentFile, strPackfile.Files.Count, packfilePair.Key, file.Name, strFile.Name);
                                    using (Stream outputStream = File.OpenWrite(Path.Combine(strOutputFolder, strFile.Name)))
                                    {
                                        using (Stream inputStream = strFile.GetStream())
                                        {
                                            inputStream.CopyTo(outputStream);
                                        }
                                        outputStream.Flush();
                                    }
                                    Console.WriteLine("done.");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Write("[{0}/{1}] Extracting {2}: {3}... ", currentFile, totalFiles, packfilePair.Key, file.Name);
                        using (Stream outputStream = File.OpenWrite(Path.Combine(options.Output, packfilePair.Key, file.Name)))
                        {
                            using (Stream inputStream = file.GetStream())
                            {
                                inputStream.CopyTo(outputStream);
                            }
                            outputStream.Flush();
                        }
                        Console.WriteLine("done.");
                    }
                }
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
