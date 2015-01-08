using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public static class Packfile
    {
        public static IPackfile FromStream(Stream stream, bool isStr2)
        {
            stream.Seek(0, SeekOrigin.Begin);
            uint descriptor = stream.ReadUInt32();

            if (descriptor != 0x51890ACE)
                throw new Exception("The input is not a packfile!");

            uint version = stream.ReadUInt32();

            switch (version)
            {
                case 0x04: // Saints Row 2
                    return new Packfiles.Version04.Packfile(stream);

                case 0x06: // Saints Row: The Third
                    throw new Exception("Saints Row: The Third packfiles are not supported.");

                case 0x0A: // Saints Row IV & Saints Row: Gat out of Hell
                    return new Packfiles.Version0A.Packfile(stream, isStr2);

                default:
                    throw new Exception(String.Format("Unsupported packfile version: {0:X4}", version));
            }
        }
    }
}
