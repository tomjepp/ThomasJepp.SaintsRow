using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using ThomasJepp.SaintsRow.Stream2;

namespace RepackTest
{
    class Program
    {
        internal class PackageOptions
        {
            public bool Compress { get; set; }
            public bool Condense { get; set; }

            public PackageOptions(bool compress, bool condense)
            {
                Compress = compress;
                Condense = condense;
            }
        }

        static Dictionary<string, PackageOptions> OptionsList = new Dictionary<string, PackageOptions>
        {
            { "characters.vpp_pc", new PackageOptions(false, false) },
            { "customize_item.vpp_pc", new PackageOptions(false, false) },
            { "customize_player.vpp_pc", new PackageOptions(false, false) },
            { "cutscene_sounds.vpp_pc", new PackageOptions(false, false) },
            { "cutscene_tables.vpp_pc", new PackageOptions(true, false) },
            { "cutscenes.vpp_pc", new PackageOptions(false, false) },
            { "da_tables.vpp_pc", new PackageOptions(true, false) },
            { "decals.vpp_pc", new PackageOptions(false, false) },
            { "dlc3.vpp_pc", new PackageOptions(false, false) },
            { "dlc4.vpp_pc", new PackageOptions(false, false) },
            { "effects.vpp_pc", new PackageOptions(false, false) },
            { "high_mips.vpp_pc", new PackageOptions(false, false) },
            { "interface.vpp_pc", new PackageOptions(false, false) },
            { "interface_startup.vpp_pc", new PackageOptions(false, false) },
            { "items.vpp_pc", new PackageOptions(false, false) },
            { "misc.vpp_pc", new PackageOptions(true, false) },
            { "misc_tables.vpp_pc", new PackageOptions(true, false) },
            { "patch_compressed.vpp_pc", new PackageOptions(true, false) },
            { "patch_uncompressed.vpp_pc", new PackageOptions(false, false) },
            { "player_morph.vpp_pc", new PackageOptions(false, false) },
            { "player_taunts.vpp_pc", new PackageOptions(false, false) },
            { "preload_anim.vpp_pc", new PackageOptions(false, true) },
            { "preload_effects.vpp_pc", new PackageOptions(false, false) },
            { "preload_items.vpp_pc", new PackageOptions(false, false) },
            { "preload_rigs.vpp_pc", new PackageOptions(false, true) },
            { "shaders.vpp_pc", new PackageOptions(true, false) },
            { "skybox.vpp_pc", new PackageOptions(false, false) },
            { "sound_turbo.vpp_pc", new PackageOptions(true, false) },
            { "soundboot.vpp_pc", new PackageOptions(true, true) },
            { "sounds.vpp_pc", new PackageOptions(false, false) },
            { "sounds_common.vpp_pc", new PackageOptions(false, false) },
            { "sr3_city_0.vpp_pc", new PackageOptions(false, false) },
            { "sr3_city_1.vpp_pc", new PackageOptions(false, false) },
            { "sr3_city_missions.vpp_pc", new PackageOptions(false, false) },
            { "startup.vpp_pc", new PackageOptions(false, false) },
            { "superpowers.vpp_pc", new PackageOptions(false, false) },
            { "vehicles.vpp_pc", new PackageOptions(false, false) },
            { "vehicles_preload.vpp_pc", new PackageOptions(true, false) },
            { "voices.vpp_pc", new PackageOptions(false, false) },
        };

        static void Main(string[] args)
        {
            string src = @"D:\Gaming\Saints Row 4\test\in";
            string temp = @"D:\Gaming\Saints Row 4\test";
            string dst = @"D:\Gaming\Saints Row 4\test\out";

            string[] packfileFolders = Directory.GetDirectories(src);

            foreach (string packfileFolder in packfileFolders)
            {
                string pfTemp = Path.Combine(temp, Path.GetFileName(packfileFolder));

                Directory.CreateDirectory(pfTemp);

                // Copy raw files
                string[] pfFiles = Directory.GetFiles(packfileFolder);
                foreach (string file in pfFiles)
                {
                    File.Copy(file, Path.Combine(pfTemp, Path.GetFileName(file)));
                }

                string[] asmFiles = Directory.GetFiles(pfTemp, "*.asm_pc");


                foreach (string asmFile in asmFiles)
                {
                    Stream2File asm = null;
                    using (Stream stream = File.OpenRead(asmFile))
                    {
                        asm = new Stream2File(stream);
                    }

                    int count = 0;
                    foreach (var container in asm.Containers)
                    {
                        count++;
                        string str2Name = Path.ChangeExtension(container.Name, ".str2_pc");
                        string str2Src = Path.Combine(packfileFolder, str2Name);

                        Console.Write("[{0}/{1}] Building {2}...", count, asm.Containers.Count, str2Src.Remove(0, src.Length + 1));
                        if (Directory.Exists(str2Src))
                        {
                            string outputFile = Path.Combine(pfTemp, Path.GetFileName(str2Src));
                            ProcessStartInfo psi = new ProcessStartInfo(@"D:\Development\SaintsRow\bin\Release\ThomasJepp.SaintsRow.BuildPackfile.exe", String.Format("sriv \"{0}\" \"{1}\" /asm:\"{2}\"", str2Src, outputFile, asmFile));
                            psi.CreateNoWindow = true;
                            psi.WindowStyle = ProcessWindowStyle.Hidden;
                            Process p = Process.Start(psi);
                            p.WaitForExit();
                            Console.WriteLine(" OK");
                        }
                        else
                        {
                            Console.WriteLine(" not found!");
                        }
                    }
                }

                Console.WriteLine("Building {0}...", Path.GetFileName(packfileFolder));

                var options = OptionsList[Path.GetFileName(packfileFolder)];

                ProcessStartInfo packpsi = new ProcessStartInfo(@"D:\Development\SaintsRow\bin\Release\ThomasJepp.SaintsRow.BuildPackfile.exe", String.Format("sriv \"{0}\" \"{1}\" /condensed:{2} /compressed:{3}", pfTemp, Path.Combine(dst, Path.GetFileName(packfileFolder)), options.Condense, options.Compress));
                packpsi.CreateNoWindow = true;
                packpsi.WindowStyle = ProcessWindowStyle.Hidden;
                Process packProcess = Process.Start(packpsi);
            }

            Console.WriteLine("Done.");
        }
    }
}
