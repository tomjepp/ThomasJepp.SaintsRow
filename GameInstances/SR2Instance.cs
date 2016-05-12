using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.GameInstances
{
    public class SR2Instance : IGameInstance
    {
        public GameSteamID Game
        {
            get { return SaintsRow.GameSteamID.SaintsRow2; }
        }

        private string _GamePath = Utility.GetGamePath(SaintsRow.GameSteamID.SaintsRow2);

        public string GamePath
        {
            get { return _GamePath; }
        }

        private string[] PackfilesToTry = new string[] { "patch.vpp_pc", "anims.vpp_pc", "audio.vpp_pc", "chunks1.vpp_pc", "chunks2.vpp_pc", "chunks3.vpp_pc", "chunks4.vpp_pc", "city_load.vpp_pc", "common.vpp_pc", "cutscenes.vpp_pc", "hmaps.vpp_pc", "meshes.vpp_pc", "music1.vpp_pc", "music2.vpp_pc", "music3.vpp_pc", "music4.vpp_pc", "pegs.vpp_pc", "decals.vpp_pc", "dlc1.vpp_pc", "dlc2.vpp_pc", "dlc3.vpp_pc", "effects.vpp_pc", "high_mips.vpp_pc", "interface.vpp_pc", "items.vpp_pc", "misc.vpp_pc", "misc_tables.vpp_pc", "player_morph.vpp_pc", "player_taunts.vpp_pc", "shaders.vpp_pc", "skybox.vpp_pc", "sound_turbo.vpp_pc", "sounds.vpp_pc", "sounds_common.vpp_pc", "sr3_city_0.vpp_pc", "sr3_city_1.vpp_pc", "sr3_city_missions.vpp_pc", "superpowers.vpp_pc", "vehicles.vpp_pc", "voices.vpp_pc" };

        public Packfiles.IPackfile OpenPackfile(string name)
        {
            string packfilePath = Path.Combine(GamePath, name);

            if (!File.Exists(packfilePath))
                return null;

            Stream s = File.OpenRead(packfilePath);
            return Packfiles.Packfile.FromStream(s, false);
        }

        public System.IO.Stream OpenLooseFile(string name)
        {
            string loosePath = Path.Combine(GamePath, name);
            if (File.Exists(loosePath))
            {
                Stream s = File.OpenRead(loosePath);
                return s;
            }
            else
            {
                return null;
            }
        }

        public System.IO.Stream OpenPackfileFile(string name)
        {
            foreach (string packfileToTry in PackfilesToTry)
            {
                Stream s = OpenPackfileFile(name, packfileToTry);
                if (s == null)
                    continue;
                else
                    return s;
            }

            return null;
        }

        public System.IO.Stream OpenPackfileFile(string name, string packfile)
        {
            using (Packfiles.IPackfile pf = OpenPackfile(packfile))
            {
                if (pf == null)
                    return null;

                foreach (Packfiles.IPackfileEntry entry in pf.Files)
                {
                    if (entry.Name == name)
                    {
                        return entry.GetStream();
                    }
                }

                return null;
            }
        }

        public System.IO.Stream OpenPackfileFile(string name, Packfiles.IPackfile packfile)
        {
            foreach (Packfiles.IPackfileEntry entry in packfile.Files)
            {
                if (entry.Name == name)
                {
                    return entry.GetStream();
                }
            }

            return null;
        }

        public Dictionary<string, FileSearchResult> SearchForFiles(string pattern)
        {
            Dictionary<string, FileSearchResult> results = new Dictionary<string, FileSearchResult>();

            bool isPrefix = pattern.EndsWith("*");
            string searchString = null;
            if (isPrefix)
                searchString = pattern.Substring(0, pattern.Length - 1).ToLowerInvariant();
            else
                searchString = pattern.Substring(1, pattern.Length - 1).ToLowerInvariant();

            foreach (string packfileToTry in PackfilesToTry)
            {
                using (var pf = OpenPackfile(packfileToTry))
                {
                    foreach (var entry in pf.Files)
                    {
                        string name = entry.Name.ToLowerInvariant();

                        if ((isPrefix && name.StartsWith(searchString)) || (!isPrefix && name.EndsWith(searchString)))
                        {
                            if (results.ContainsKey(name))
                                continue;

                            results.Add(name, new FileSearchResult(this, entry.Name, packfileToTry));
                        }
                    }
                }
            }

            return results;
        }
    }
}
