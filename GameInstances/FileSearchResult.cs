using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ThomasJepp.SaintsRow.GameInstances
{
    public class FileSearchResult
    {
        public FileSearchResult(IGameInstance instance, string filename) : this(instance, filename, null)
        {

        }

        public FileSearchResult(IGameInstance instance, string filename, string packfile)
        {
            GameInstance = instance;
            Filename = filename;
            Packfile = packfile;
        }

        public string Filename { get; set; }
        public string Packfile { get; set; }

        private IGameInstance GameInstance;

        public Stream GetStream()
        {
            if (Packfile == null)
                return GameInstance.OpenLooseFile(Filename);
            else
                return GameInstance.OpenPackfileFile(Filename, Packfile);
        }
    }
}
