using System;
using System.IO;

using CmdLine;
using ThomasJepp.SaintsRow.Packfiles;
using ThomasJepp.SaintsRow.Stream2;

namespace ThomasJepp.SaintsRow.BuildPackfile
{
    class Program
    {
        [CommandLineArguments(Program = "ThomasJepp.SaintsRow.BuildPackfile", Title = "Saints Row Packfile Builder", Description = "Builds Saints Row PC packfiles (vpp_pc and str2_pc files). Supports Saints Row IV.")]
        internal class Options
        {
            [CommandLineParameter(Name = "game", ParameterIndex = 1, Required = true, Description = "The game you wish to build a packfile for. Valid options are \"sr2\", \"srtt\" and \"sriv\".")]
            public string Game { get; set; }

            [CommandLineParameter(Name = "source", ParameterIndex = 2, Required = true, Description = "A folder containing files to pack.")]
            public string Source { get; set; }

            [CommandLineParameter(Name = "output", ParameterIndex = 3, Required = true, Description = "The file to create.")]
            public string Output { get; set; }

            [CommandLineParameter(Command = "condensed", Description = "Should the output data be condensed? This is mostly used for str2_pc files. Saints Row 2 files cannot be condensed. If not specified, \"\auto\" is the default.", ValueExample = "true|false|auto", Default = "auto")]
            public string Condensed { get; set; }

            [CommandLineParameter(Command = "compressed", Description = "Should the output data be compressed? This is usually enabled for str2_pc files and for vpp_pc files that contain highly compressible data such as XTBL files. If not specified, \"\auto\" is the default.", ValueExample = "true|false|auto",Default="auto")]
            public string Compressed { get; set; }

            [CommandLineParameter(Command = "asm", Description = "The asm_pc file to update with new data from this packfile. This should only be used in Saints Row IV or Saints Row: The Third mode, and will automatically update the specified ASM file. If you are building a str2_pc file, this should be specified. It has no effect for vpp_pc files.", ValueExample = "<asm_pc file>")]
            public string AsmFile { get; set; }
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

            IPackfile packfile = null;
            Stream2File asm = null;

            Console.WriteLine("Building {0} using data from {1}.", options.Output, options.Source);
            switch (options.Game.ToLowerInvariant())
            {
                case "sr2":
                    throw new NotImplementedException();
                    break;
                case "srtt":
                    throw new NotImplementedException();
                    break;
                case "sriv":
                    packfile = new Packfiles.Version0A.Packfile();
                    if (Path.GetExtension(options.Output) == ".str2_pc")
                    {
                        packfile.IsCondensed = true;
                        packfile.IsCompressed = true;
                        if (options.AsmFile != null)
                        {
                            Console.WriteLine("Will update asm_pc file {0} with data for new package.", options.AsmFile);
                            using (Stream asmStream = File.OpenRead(options.AsmFile))
                            {
                                asm = new Stream2File(asmStream);
                            }

                        }
                    }
                    break;
            }

            if (options.Condensed.ToLowerInvariant() == "true")
                packfile.IsCondensed = true;
            else if (options.Condensed.ToLowerInvariant() == "false")
                packfile.IsCondensed = false;

            if (options.Compressed.ToLowerInvariant() == "true")
                packfile.IsCompressed = true;
            else if (options.Compressed.ToLowerInvariant() == "false")
                packfile.IsCompressed = false;

            Container thisContainer = null;

            if (asm != null)
            {
                string containerName = Path.GetFileNameWithoutExtension(options.Output);

                foreach (var container in asm.Containers)
                {
                    string tempContainerName = Path.GetFileNameWithoutExtension(container.Name);
                    if (tempContainerName == containerName)
                    {
                        thisContainer = container;
                        break;
                    }
                }

                if (thisContainer == null)
                    throw new Exception(String.Format("Unable to find container {0} in asm_pc file {1}.", containerName, options.AsmFile));

                foreach (Primitive primitive in thisContainer.Primitives)
                {
                    string primitiveFile = Path.Combine(options.Source, primitive.Name);
                    if (!File.Exists(primitiveFile))
                    {
                        Console.WriteLine("Unable to find primitive {0} for container {1}", containerName, primitive.Name);
                        continue;
                    }

                    string filename = Path.GetFileName(primitiveFile);

                    Console.Write("Adding {0}... ", filename);
                    Stream stream = File.OpenRead(primitiveFile);
                    packfile.AddFile(stream, filename);
                    Console.WriteLine("done.");

                    string extension = Path.GetExtension(primitiveFile);
                    string gpuExtension = "";
                    switch (extension)
                    {
                        default:
                            {
                                if (extension.StartsWith(".c"))
                                    gpuExtension = ".g" + extension.Remove(0, 2);
                                break;
                            }
                    }


                    string gpuFile = Path.ChangeExtension(primitiveFile, gpuExtension);
                    if (File.Exists(gpuFile))
                    {
                        string gpuFilename = Path.GetFileName(gpuFile);
                        Console.Write("Adding {0}... ", gpuFilename);
                        Stream gpuStream = File.OpenRead(gpuFile);
                        packfile.AddFile(gpuStream, gpuFilename);
                        Console.WriteLine("done.");
                    }
                }
            }
            else
            {
                string[] files = Directory.GetFiles(options.Source);
                foreach (string file in files)
                {
                    string filename = Path.GetFileName(file);

                    Console.Write("Adding {0}... ", filename);
                    Stream stream = File.OpenRead(file);
                    packfile.AddFile(stream, filename);
                    Console.WriteLine("done.");
                }
            }

            using (Stream output = File.Open(options.Output, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                Console.Write("Writing packfile to {0}... ", options.Output);
                packfile.Save(output);
                Console.WriteLine("done.");
            }

            if (asm != null)
            {
                Console.Write("Updating asm_pc file {0}... ", options.AsmFile);
                packfile.Update(thisContainer);

                using (Stream asmStream = File.OpenWrite(options.AsmFile))
                {
                    asm.Save(asmStream);
                }
                Console.WriteLine("done.");
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
