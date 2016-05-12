using System;
using System.Collections.Generic;
using System.IO;

using ThomasJepp.SaintsRow.Steam;

namespace ThomasJepp.SaintsRow
{
    public static class Utility
    {
        private static string GetRegistryEntry(string key, string value)
        {
            return (string)Microsoft.Win32.Registry.GetValue(key, value, null);
        }

        public static string GetSteamPath()
        {
            string steamPath = GetRegistryEntry(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath");
            if (steamPath == null)
                steamPath = GetRegistryEntry(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath");

            return steamPath;
        }

        public static string GetGamePath(GameSteamID gameId)
        {
            int id = (int)gameId;

            string steamPath = GetSteamPath();

            if (steamPath != null)
            {
                string appManifestFile = Path.Combine(steamPath, "SteamApps", String.Format("appmanifest_{0}.acf", id));
                if (File.Exists(appManifestFile))
                {
                    KeyValues manifestKv;
                    using (Stream s = File.OpenRead(appManifestFile))
                    {
                        manifestKv = new KeyValues(s);
                    }

                    Dictionary<string, object> appState = (Dictionary<string, object>)manifestKv.Items["AppState"];
                    string installdir = (string)appState["installdir"];
                    string path = Path.Combine(steamPath, "SteamApps", "common", installdir);
                    if (Directory.Exists(path))
                        return path;
                }
                else
                {
                    string libraryFoldersFile = Path.Combine(steamPath, "SteamApps", String.Format("libraryfolders.vdf"));
                    if (File.Exists(libraryFoldersFile))
                    {
                        KeyValues kv;
                        using (Stream s = File.OpenRead(libraryFoldersFile))
                        {
                            kv = new KeyValues(s);
                        }

                        Dictionary<string, object> libraryFolders = (Dictionary<string, object>)kv.Items["LibraryFolders"];
                        int folderId = 0;
                        while (true)
                        {
                            folderId++;
                            if (!libraryFolders.ContainsKey(folderId.ToString()))
                                break;

                            string extraLibrary = (string)libraryFolders[folderId.ToString()];

                            appManifestFile = Path.Combine(extraLibrary, "steamapps", String.Format("appmanifest_{0}.acf", id));
                            if (File.Exists(appManifestFile))
                            {
                                KeyValues manifestKv;
                                using (Stream s = File.OpenRead(appManifestFile))
                                {
                                    manifestKv = new KeyValues(s);
                                }

                                Dictionary<string, object> appState = (Dictionary<string, object>)manifestKv.Items["AppState"];
                                string installdir = (string)appState["installdir"];
                                string path = Path.Combine(extraLibrary, "steamapps", "common", installdir);
                                if (Directory.Exists(path))
                                    return path;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
