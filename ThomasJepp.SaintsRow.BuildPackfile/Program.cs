using System;
using System.IO;

using CmdLine;
using ThomasJepp.SaintsRow.Packfiles;

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

            [CommandLineParameter(Command = "condensed", Description = "Should the output data be condensed? This is mostly used for str2_pc files. Saints Row 2 files cannot be condensed. If not specified, \"\auto\" is the default.", ValueExample="true|false|auto")]
            public string Condensed { get; set; }

            [CommandLineParameter(Command = "compressed", Description = "Should the output data be compressed? This is usually enabled for str2_pc files and for vpp_pc files that contain highly compressible data such as XTBL files. If not specified, \"\auto\" is the default.", ValueExample = "true|false|auto")]
            public string Compressed { get; set; }
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
                    }
                    break;
            }

            if (options.Condensed == "true")
                packfile.IsCondensed = true;
            else if (options.Condensed == "false")
                packfile.IsCondensed = false;

            if (options.Compressed == "true")
                packfile.IsCompressed = true;
            else if (options.Compressed == "false")
                packfile.IsCompressed = false;

            string[] files = Directory.GetFiles(options.Source);
            foreach (string file in files)
            {
                Stream stream = File.OpenRead(file);
                string filename = Path.GetFileName(file);
                packfile.AddFile(stream, filename);
            }

            using (Stream output = File.Open(options.Output, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                packfile.Save(output);
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
