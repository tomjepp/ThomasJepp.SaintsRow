using System;
using System.IO;

using CmdLine;
using ThomasJepp.SaintsRow;

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

            using (Stream stream = File.OpenRead(options.Source))
            {
                Stream2File file = new Stream2File(stream);
            }
        }
    }
}
