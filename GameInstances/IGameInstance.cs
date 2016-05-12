using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ThomasJepp.SaintsRow.Packfiles;

namespace ThomasJepp.SaintsRow.GameInstances
{
    public interface IGameInstance
    {
        GameSteamID Game { get; }

        string GamePath { get; }

        IPackfile OpenPackfile(string name);
        Stream OpenLooseFile(string name);
        Stream OpenPackfileFile(string name);
        Stream OpenPackfileFile(string name, string packfile);
        Stream OpenPackfileFile(string name, IPackfile packfile);
        Dictionary<string, FileSearchResult> SearchForFiles(string pattern);
    }
}
