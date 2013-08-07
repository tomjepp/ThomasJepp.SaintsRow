using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public interface IPackfile : IDisposable
    {
        List<IPackfileEntry> Files { get; }
        IPackfileEntry this[int i] { get; }

        bool IsCompressed { get; }
        bool IsCondensed { get; }
    }
}
