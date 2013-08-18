using System;
using System.Collections.Generic;
using System.IO;

using ThomasJepp.SaintsRow.Stream2;

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
        void Update(Container container);
    }
}
