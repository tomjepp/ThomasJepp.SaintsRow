using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CmdLine;

using ThomasJepp.SaintsRow.Packfiles;
using ThomasJepp.SaintsRow.Stream2;

namespace ThomasJepp.SaintsRow.Stream2Update
{
    static class Program
    {
        [CommandLineArguments(Program = "ThomasJepp.SaintsRow.Stream2Update", Title = "Saints Row Stream2 Update Tool", Description = "Fetches the required ASM files for your mods and updates them as necessary. Supports Saints Row IV.")]
        internal class Options
        {
            [CommandLineParameter(Name = "source", ParameterIndex = 1, Required = false, Description = "Your Saints Row IV install folder. If blank, this will be autodetected.")]
            public string Source { get; set; }
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

                Console.ReadLine();
                return;
            }

            if (options.Source == null)
            {
                string sriv = ThomasJepp.SaintsRow.Utility.GetGamePath(Game.SaintsRowIV);
                string srgooh = ThomasJepp.SaintsRow.Utility.GetGamePath(Game.SaintsRowGatOutOfHell);

                int gameCount = 0, srivNum = 0, srgoohNum = 0;
                Console.WriteLine("Detected the following games:");
                if (sriv != null)
                {
                    gameCount++;
                    srivNum = gameCount;
                    Console.WriteLine("{0}. Saints Row IV: {1}", gameCount, sriv);
                }
                if (srgooh != null)
                {
                    gameCount++;
                    srgoohNum = gameCount;
                    Console.WriteLine("{0}. Saints Row Gat Out Of Hell: {1}", gameCount, srgooh);
                }

                Console.WriteLine();
                while (true)
                {
                    Console.Write("Which game do you want to update? (enter the number) ");
                    ConsoleKeyInfo input = Console.ReadKey();
                    Console.WriteLine();
                    Console.WriteLine();
                    if (input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1)
                    {
                        if (srivNum == 1)
                        {
                            options.Source = sriv;
                            Console.WriteLine("Updating Saints Row IV files.");
                        }
                        else if (srgoohNum == 1)
                        {
                            options.Source = srgooh;
                            Console.WriteLine("Updating Saints Row: Gat Out Of Hell files.");
                        }
                    }
                    else if (input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2)
                    {
                        if (srivNum == 2)
                        {
                            options.Source = sriv;
                            Console.WriteLine("Updating Saints Row IV files.");
                        }
                        else if (srgoohNum == 2)
                        {
                            options.Source = srgooh;
                            Console.WriteLine("Updating Saints Row: Gat Out Of Hell files.");
                        }
                    }

                    if (options.Source != null)
                        break;
                }
            }


            if (options.Source == null)
            {
                Console.WriteLine("Couldn't find the Saints Row IV folder?");

                Console.WriteLine();
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                return;
            }

            string str2Dir = options.Source;
            if (Directory.Exists(Path.Combine(options.Source, "mods")))
                str2Dir = Path.Combine(options.Source, "mods");

            string[] str2Paths = Directory.GetFiles(str2Dir, "*.str2_pc");

            List<string> str2Files = new List<string>();

            foreach (string str2Path in str2Paths)
            {
                str2Files.Add(Path.GetFileName(str2Path));
            }

            if (str2Files.Count == 0)
            {
                Console.WriteLine("No str2_pc files found - no update needed.");

                Console.WriteLine();
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                return;
            }

            string packfileCache = Path.Combine(options.Source, "packfiles", "pc", "cache");

            Dictionary<string, Stream2File> asmsToSave = new Dictionary<string, Stream2File>();

            string[] packfiles = Directory.GetFiles(packfileCache, "*.vpp_pc");
            int vppCount = 0;
            foreach (string packfilePath in packfiles)
            {
                vppCount ++;
                Console.WriteLine("[{0}/{1}] Checking {2}...", vppCount, packfiles.Length, Path.GetFileName(packfilePath));

                using (Stream stream = File.OpenRead(packfilePath))
                {
                    using (IPackfile packfile = Packfile.FromStream(stream, false))
                    {
                        foreach (var packedFile in packfile.Files)
                        {
                            if (Path.GetExtension(packedFile.Name) != ".asm_pc")
                                continue;

                            using (Stream asmStream = packedFile.GetStream())
                            {
                                Stream2File asm = new Stream2File(asmStream);

                                foreach (var container in asm.Containers)
                                {
                                    string containerName = Path.ChangeExtension(container.Name, ".str2_pc");
                                    if (str2Files.Contains(containerName))
                                    {
                                        if (!asmsToSave.ContainsKey(packedFile.Name))
                                        {
                                            asmsToSave.Add(packedFile.Name, asm);
                                        }

                                        Console.Write(" - Updating {0} - {1}...", packedFile.Name, containerName);
                                        using (Stream str2Stream = File.OpenRead(Path.Combine(str2Dir, containerName)))
                                        {
                                            using (IPackfile str2 = Packfile.FromStream(str2Stream, true))
                                            {
                                                str2.Update(container);
                                            }
                                        }
                                        Console.WriteLine(" done.");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine();

            Console.WriteLine("Writing updated asm_pc files...");
            int count = 0;
            foreach (var asmPair in asmsToSave)
            {
                count++;
                Console.Write("[{0}/{1}] Saving {2}...", count, asmsToSave.Count, asmPair.Key);
                string outPath = Path.Combine(str2Dir, asmPair.Key);

                using (Stream outStream = File.OpenWrite(outPath))
                {
                    asmPair.Value.Save(outStream);
                }
                Console.WriteLine(" done.");
            }

            Console.WriteLine("Done.");

            Console.WriteLine();
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
