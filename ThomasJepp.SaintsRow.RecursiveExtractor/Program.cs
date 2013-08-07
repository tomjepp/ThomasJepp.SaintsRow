using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.RecursiveExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("ThomasJepp.SaintsRow.RecursiveExtractor <input folder> <output folder>");
                return;
            }

            string inputFolder = args[0];
            string outputFolder = args[1];

            Directory.CreateDirectory(outputFolder);

            string[] files = Directory.GetFiles(inputFolder);
            
            foreach (string file in files)
            {
                Console.WriteLine(Path.GetExtension(file));
            }
        }
    }
}
