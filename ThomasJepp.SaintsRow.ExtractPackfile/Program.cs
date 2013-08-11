using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CmdLine;
using ThomasJepp.SaintsRow.Packfiles;

namespace ThomasJepp.SaintsRow.ExtractPackfile
{
    class Program
    {
        [CommandLineArguments(Program="ThomasJepp.SaintsRow.ExtractPackfile", Title="Saints Row Packfile Extractor", Description="Extracts Saints Row PC Packfiles (vpp_pc and str2_pc files). Supports Saints Row IV.")]
        internal class Options
        {
            [CommandLineParameter(Name="source", ParameterIndex=1, Required=true, Description="The packfile to extract.")]
            public string Source { get; set; }

            [CommandLineParameter(Name="output", ParameterIndex=2, Required=false, Description="The folder to extract the packfile to. This will be created if it does not already exist. If not specified, the packfile will be extracted to a new folder with the same name in the current directory.")]
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

            using (Stream stream = File.OpenRead(options.Source))
            {
                var packfile = Packfile.FromStream(stream);

                string folderName = (options.Output != null) ? options.Output : Path.GetFileName(args[0]);

                Console.WriteLine("Extracting {0} to {1}.", options.Source, folderName);

                Directory.CreateDirectory(folderName);

                int currentFile = 0;
                foreach (IPackfileEntry entry in packfile.Files)
                {
                    currentFile++;

                    Console.Write("[{0}/{1}] Extracting {2}... ", currentFile, packfile.Files.Count, entry.Name);
                    using (Stream outputStream = File.OpenWrite(Path.Combine(folderName, entry.Name)))
                    {
                        using (Stream inputStream = entry.GetStream())
                        {
                            inputStream.CopyTo(outputStream);
                        }
                        outputStream.Flush();
                    }
                    Console.WriteLine("done.");
                }

#if DEBUG
                Console.ReadLine();
#endif
            }
        }
    }
}
