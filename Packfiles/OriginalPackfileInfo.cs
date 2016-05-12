using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public class PackageOptions
    {
        public bool Compress { get; set; }
        public bool Condense { get; set; }

        public PackageOptions(bool compress, bool condense)
        {
            Compress = compress;
            Condense = condense;
        }
    }

    public static class OriginalPackfileInfo
    {
        public static Dictionary<GameSteamID, Dictionary<string, PackageOptions>> OptionsList = new Dictionary<GameSteamID,Dictionary<string,PackageOptions>>
        {
            {
                GameSteamID.SaintsRowTheThird,
                new Dictionary<string, PackageOptions>
                {
                    { "characters.vpp_pc", new PackageOptions(false, false) },
                    { "customize_item.vpp_pc", new PackageOptions(false, false) },
                    { "customize_player.vpp_pc", new PackageOptions(false, false) },
                    { "cutscene_sounds.vpp_pc", new PackageOptions(false, false) },
                    { "cutscene_tables.vpp_pc", new PackageOptions(true, false) },
                    { "cutscenes.vpp_pc", new PackageOptions(false, false) },
                    { "da_tables.vpp_pc", new PackageOptions(true, false) },
                    { "decals.vpp_pc", new PackageOptions(false, false) },
                    { "dlc1.vpp_pc", new PackageOptions(false, false) },
                    { "dlc2.vpp_pc", new PackageOptions(false, false) },
                    { "dlc3.vpp_pc", new PackageOptions(false, false) },
                    { "effects.vpp_pc", new PackageOptions(false, false) },
                    { "high_mips.vpp_pc", new PackageOptions(true, false) },
                    { "interface.vpp_pc", new PackageOptions(false, false) },
                    { "interface_startup.vpp_pc", new PackageOptions(false, false) },
                    { "items.vpp_pc", new PackageOptions(false, false) },
                    { "misc.vpp_pc", new PackageOptions(true, false) },
                    { "misc_tables.vpp_pc", new PackageOptions(true, false) },
                    { "patch_compressed.vpp_pc", new PackageOptions(true, false) },
                    { "patch_uncompressed.vpp_pc", new PackageOptions(false, false) },
                    { "player_morph.vpp_pc", new PackageOptions(false, false) },
                    { "preload_anim.vpp_pc", new PackageOptions(false, true) },
                    { "preload_effects.vpp_pc", new PackageOptions(false, false) },
                    { "preload_items.vpp_pc", new PackageOptions(false, false) },
                    { "preload_rigs.vpp_pc", new PackageOptions(false, true) },
                    { "shaders.vpp_pc", new PackageOptions(true, false) },
                    { "skybox.vpp_pc", new PackageOptions(false, false) },
                    { "soundboot.vpp_pc", new PackageOptions(true, true) },
                    { "sounds.vpp_pc", new PackageOptions(false, false) },
                    { "sounds_common.vpp_pc", new PackageOptions(false, false) },
                    { "sound_turbo.vpp_pc", new PackageOptions(true, false) },
                    { "sr3_city_0.vpp_pc", new PackageOptions(false, false) },
                    { "sr3_city_1.vpp_pc", new PackageOptions(false, false) },
                    { "sr3_city_missions.vpp_pc", new PackageOptions(false, false) },
                    { "startup.vpp_pc", new PackageOptions(true, false) },
                    { "vehicles.vpp_pc", new PackageOptions(false, false) },
                    { "vehicles_preload.vpp_pc", new PackageOptions(true, false) },
                    { "voices.vpp_pc", new PackageOptions(false, false) },
                }
            },
            {
                GameSteamID.SaintsRowIV,
                new Dictionary<string, PackageOptions>
                {
                    { "characters.vpp_pc", new PackageOptions(false, false) },
                    { "customize_item.vpp_pc", new PackageOptions(false, false) },
                    { "customize_player.vpp_pc", new PackageOptions(false, false) },
                    { "cutscene_sounds.vpp_pc", new PackageOptions(false, false) },
                    { "cutscene_tables.vpp_pc", new PackageOptions(true, false) },
                    { "cutscenes.vpp_pc", new PackageOptions(false, false) },
                    { "da_tables.vpp_pc", new PackageOptions(true, false) },
                    { "decals.vpp_pc", new PackageOptions(false, false) },
                    { "dlc1.vpp_pc", new PackageOptions(false, false) },
                    { "dlc2.vpp_pc", new PackageOptions(false, false) },
                    { "dlc3.vpp_pc", new PackageOptions(false, false) },
                    { "dlc4.vpp_pc", new PackageOptions(false, false) },
                    { "dlc5.vpp_pc", new PackageOptions(false, false) },
                    { "dlc6.vpp_pc", new PackageOptions(false, false) },
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
                }
            },
            {
                GameSteamID.SaintsRowGatOutOfHell,
                new Dictionary<string, PackageOptions>
                {
                    { "characters.vpp_pc", new PackageOptions(false, false) },
                    { "customize_item.vpp_pc", new PackageOptions(false, false) },
                    { "customize_player.vpp_pc", new PackageOptions(false, false) },
                    { "cutscene_sounds.vpp_pc", new PackageOptions(false, false) },
                    { "cutscene_tables.vpp_pc", new PackageOptions(true, false) },
                    { "cutscenes.vpp_pc", new PackageOptions(false, false) },
                    { "da_tables.vpp_pc", new PackageOptions(true, false) },
                    { "decals.vpp_pc", new PackageOptions(false, false) },
                    { "effects.vpp_pc", new PackageOptions(false, false) },
                    { "high_mips.vpp_pc", new PackageOptions(false, false) },
                    { "interface.vpp_pc", new PackageOptions(false, false) },
                    { "interface_startup.vpp_pc", new PackageOptions(false, false) },
                    { "items.vpp_pc", new PackageOptions(false, false) },
                    { "misc.vpp_pc", new PackageOptions(true, false) },
                    { "misc_tables.vpp_pc", new PackageOptions(true, false) },
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
                    { "sr4_5_city_0.vpp_pc", new PackageOptions(false, false) },
                    { "sr4_5_city_1.vpp_pc", new PackageOptions(false, false) },
                    { "sr4_5_city_missions.vpp_pc", new PackageOptions(false, false) },
                    { "startup.vpp_pc", new PackageOptions(false, false) },
                    { "superpowers.vpp_pc", new PackageOptions(false, false) },
                    { "vehicles.vpp_pc", new PackageOptions(false, false) },
                    { "vehicles_preload.vpp_pc", new PackageOptions(true, false) },
                    { "voices.vpp_pc", new PackageOptions(false, false) },
                }
            },
        };
    }
}
