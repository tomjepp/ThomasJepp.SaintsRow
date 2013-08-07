using System;
using System.Collections.Generic;
using System.IO;

namespace ThomasJepp.SaintsRow.Packfiles
{
    public interface IPackfileEntry
    {
        string Name { get; }
        int Size { get; }
        Stream GetStream();
    }
}
