using System;
using System.IO;

using CmdLine;
using ThomasJepp.SaintsRow.Packfiles;

namespace ThomasJepp.SaintsRow.Stream2
{
    [CommandLineArguments(Program = "ThomasJepp.SaintsRow.Stream2", Title = "Saints Row Stream2 File Tool", Description = "Performs various actions on Stream2 files (ASM files). Supports Saints Row IV.")]
    internal class Options
    {
        [CommandLineParameter(Name = "source", ParameterIndex = 1, Required = true, Description = "The Stream2 Container to process.")]
        public string Source { get; set; }

        [CommandLineParameter(Name = "action", ParameterIndex = 2, Required = true, Description = "The action to perform. Valid actions are \"dump\" and \"update\".")]
        public string Action { get; set; }
    }

    class Program
    {
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


            switch (options.Action)
            {
                case "dump":
                    {
                        using (Stream stream = File.OpenRead(options.Source))
                        {
                            Stream2File file = new Stream2File(stream);

                            using (StreamWriter sw = new StreamWriter(options.Source + ".dump.txt"))
                            {
                                foreach (var container in file.Containers)
                                {
                                    sw.WriteLine("{0}:", container.Name);
                                    sw.WriteLine("\tContainerType: 0x{0:X2} ({0}) - {1}", container.ContainerType, file.ContainerTypes[container.ContainerType]);
                                    sw.WriteLine("\tFlags: 0x{0:X4} ({0})", (UInt16)container.Flags);
                                    sw.WriteLine("\tPrimitiveCount: 0x{0:X4} ({0})", container.PrimitiveCount);
                                    sw.WriteLine("\tPackfileBaseOffset: 0x{0:X8} ({0})", container.PackfileBaseOffset);
                                    sw.WriteLine("\tCompressionType: 0x{0:X2} ({0})", container.CompressionType);
                                    sw.WriteLine("\tStubContainerParentName: {0}", container.StubContainerParentName);
                                    sw.WriteLine("\tTotalCompressedPackfileReadSize: 0x{0:X8} ({0})", container.TotalCompressedPackfileReadSize);
                                    sw.WriteLine();
                                    for (int i = 0; i < container.PrimitiveCount; i++)
                                    {
                                        var primitive = container.Primitives[i];
                                        var sizes = container.PrimitiveSizes[i];
                                        sw.WriteLine("\t{0}:", primitive.Name);
                                        sw.WriteLine("\t\tType: 0x{0:X2} ({0}) - {1}", primitive.Data.Type, file.PrimitiveTypes[primitive.Data.Type]);
                                        sw.WriteLine("\t\tAllocator: 0x{0:X2} ({0}) - {1}", primitive.Data.Allocator, primitive.Data.Allocator == 0 ? "none" : file.AllocatorTypes[primitive.Data.Allocator]);
                                        sw.WriteLine("\t\tFlags: 0x{0:X2} ({0})", (byte)primitive.Data.Flags);
                                        sw.WriteLine("\t\tExtensionIndex: 0x{0:X2} ({0})", primitive.Data.ExtensionIndex);
                                        sw.WriteLine("\t\tCPUSize: 0x{0:X8} ({0})", primitive.Data.CPUSize);
                                        sw.WriteLine("\t\tGPUSize: 0x{0:X8} ({0})", primitive.Data.GPUSize);
                                        sw.WriteLine("\t\tAllocationGroup: 0x{0:X2} ({0})", primitive.Data.AllocationGroup);
                                        sw.WriteLine("\t\tAlt CPUSize: 0x{0:X8} ({0})", sizes.CPUSize);
                                        sw.WriteLine("\t\tAlt GPUSize: 0x{0:X8} ({0})", sizes.GPUSize);
                                    }
                                    sw.WriteLine();
                                }
                            }
                        }
                        break;
                    }
                case "update":
                    {
                        Stream2File file = null;
                        using (Stream stream = File.OpenRead(options.Source))
                        {
                            file = new Stream2File(stream);

                            string folder = Path.GetDirectoryName(options.Source);

                            foreach (var container in file.Containers)
                            {
                                string str2File = Path.ChangeExtension(container.Name, ".str2_pc");
                                string filename = Path.Combine(folder, str2File);
                                if (File.Exists(filename))
                                {
                                    Console.Write("Found {0}, updating... ", str2File);
                                    using (Stream str2Stream = File.OpenRead(filename))
                                    {
                                        IPackfile packfile = Packfile.FromStream(str2Stream);
                                        packfile.Update(container);
                                    }
                                    Console.WriteLine("done.");
                                }
                                else
                                {
                                    Console.WriteLine("Could not find {0}.", str2File);
                                }
                            }
                        }

                        using (Stream stream = File.OpenWrite(options.Source))
                        {
                            file.Save(stream);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unrecogised action!");
                        break;
                    }
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
