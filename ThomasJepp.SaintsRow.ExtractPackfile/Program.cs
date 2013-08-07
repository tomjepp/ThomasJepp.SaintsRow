using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ThomasJepp.SaintsRow.Packfiles;

namespace ThomasJepp.SaintsRow.ExtractPackfile
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream stream = File.OpenRead(args[0]))
            {
                var packfile = Packfile.FromStream(stream);

                string folderName = Path.GetFileNameWithoutExtension(args[0]);

                Console.WriteLine("Creating output: {0}", folderName);

                Directory.CreateDirectory(folderName);

                foreach (IPackfileEntry entry in packfile.Files)
                {
                    Console.Write(" - {0}... ", entry.Name);
                    using (Stream source = entry.GetStream())
                    {
                        using (Stream destination = File.OpenWrite(Path.Combine(folderName, entry.Name)))
                        {
                            source.CopyTo(destination);
                            destination.Flush();
                        }
                    }
                    Console.WriteLine("done.");
                }

                Console.WriteLine("Done.");

#if DEBUG
                Console.ReadLine();
#endif
            }
        }
    }
}
