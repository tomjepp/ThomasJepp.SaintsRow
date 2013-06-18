using System;
using System.IO;

using ThomasJepp.SaintsRow.Strings;

namespace ThomasJepp.SaintsRow.ExtractStrings
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage:\nThomasJepp.SaintsRow.ExtractStrings.exe <string file>");
                return;
            }

            string file = args[0];

            using (Stream stream = File.OpenRead(file))
            {
                StringFile stringFile = new StringFile(stream);
            }

            Console.ReadLine();
        }
    }
}
