using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.GameInstances
{
    public static class GameInstance
    {
        public static IGameInstance GetFromSteamId(GameSteamID game)
        {
            switch (game)
            {
                case GameSteamID.SaintsRow2:
                    return new SR2Instance();

                case GameSteamID.SaintsRowTheThird:
                    return new SRTTInstance();

                case GameSteamID.SaintsRowIV:
                    return new SRIVInstance();

                case GameSteamID.SaintsRowGatOutOfHell:
                    return new SRGOOHInstance();

                default:
                    throw new NotImplementedException();
            }
        }

        public static IGameInstance GetFromString(string game)
        {
            switch (game.ToLowerInvariant())
            {
                case "sr2":
                case "saintsrow2":
                    return new SR2Instance();
                    
                case "sr3":
                case "srtt":
                case "saintsrowthethird":
                    return new SRTTInstance();

                case "sr4":
                case "sriv":
                case "saintsrowiv":
                    return new SRIVInstance();

                case "gooh":
                case "srgooh":
                case "saintsrowgatoutofhell":
                    return new SRGOOHInstance();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
