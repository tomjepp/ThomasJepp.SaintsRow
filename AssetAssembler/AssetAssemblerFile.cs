using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.AssetAssembler
{
    public static class AssetAssemblerFile
    {
        public static IAssetAssemblerFile FromStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            uint descriptor = stream.ReadUInt32();

            if (descriptor != 0xBEEFFEED)
                throw new Exception("The input is not an Asset Assembler file!");

            ushort version = stream.ReadUInt16();

            stream.Seek(0, SeekOrigin.Begin);

            switch (version)
            {
                case 0x0B: // Saints Row: The Third
                    return new Version0B.AssetAssemblerFile(stream);

                case 0x0C: // Saints Row IV or Saints Row: Gat out of Hell
                    return new Version0C.AssetAssemblerFile(stream);

                default:
                    throw new NotImplementedException(String.Format("Unsupported Asset Assembler file version: {0:X4}", version));
            }
        }

        public static IAssetAssemblerFile Create(GameSteamID game)
        {
            switch (game)
            {
                case GameSteamID.SaintsRow2:
                    throw new Exception("Saints Row 2 does not have Asset Assembler files!");
                case GameSteamID.SaintsRowTheThird:
                    return Create(0x0B);
                case GameSteamID.SaintsRowIV:
                case GameSteamID.SaintsRowGatOutOfHell:
                    return Create(0x0C);
                default:
                    throw new NotImplementedException(String.Format("Unsupported game ID: {0}", game));
            }
        }

        public static IAssetAssemblerFile Create(uint version)
        {
            switch (version)
            {
                case 0x0B:
                    return new Version0B.AssetAssemblerFile();

                case 0x0C:
                    return new Version0C.AssetAssemblerFile();

                default:
                    throw new NotImplementedException(String.Format("Unsupported Asset Assembler version: {0}", version));
            }
        }
    }
}
