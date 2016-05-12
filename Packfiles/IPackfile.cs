using System;
using System.Collections.Generic;
using System.IO;

using ThomasJepp.SaintsRow.AssetAssembler;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public interface IPackfile : IDisposable
    {
        List<IPackfileEntry> Files { get; }
        IPackfileEntry this[int i] { get; }
        int Version { get; }

        bool IsCompressed { get; set; }
        bool IsCondensed { get; set; }

        void Save(Stream stream);
        void AddFile(Stream stream, string filename);
        bool ContainsFile(string filename);
        void RemoveFile(string filename);
        void RemoveFile(IPackfileEntry entry);


        void Update(IContainer container);
    }
}
