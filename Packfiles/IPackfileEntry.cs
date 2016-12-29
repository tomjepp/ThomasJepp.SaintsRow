using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public interface IPackfileEntry
    {
        string Name { get; }
        int Size { get; }
        bool IsNew { get; }
        bool HasStream { get; }
        Stream GetStream();
    }
}
