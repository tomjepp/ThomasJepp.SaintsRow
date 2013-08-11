using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public interface IPackfile : IDisposable
    {
        List<IPackfileEntry> Files { get; }
        IPackfileEntry this[int i] { get; }

        bool IsCompressed { get; set; }
        bool IsCondensed { get; set; }

        void Save(Stream stream);
        void AddFile(Stream stream, string filename);
    }
}
