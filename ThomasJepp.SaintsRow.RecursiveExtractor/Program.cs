using System;
using System.Collections.Generic;
using System.IO;

using ThomasJepp.SaintsRow.Packfiles;

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
                Directory.CreateDirectory(Path.Combine(outputFolder, packfilePair.Key));
                foreach (var file in packfilePair.Value.Files)
                {
                    currentFile++;
                    if (Path.GetExtension(file.Name) == ".str2_pc")
                    {
                        string strOutputFolder = Path.Combine(outputFolder, packfilePair.Key, file.Name);
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
                        using (Stream outputStream = File.OpenWrite(Path.Combine(outputFolder, packfilePair.Key, file.Name)))
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
