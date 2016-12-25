using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod.Sections.Player
{
    [StructLayout(LayoutKind.Explicit, Size = 0x04)]
    public struct CollectiblePersonaSavedInfo // collectible_persona_saved_info
    {
        [FieldOffset(0x00)] // length = 0x4, bit offset: 0
        public uint PlayerOrbPercentCollectionLine;

    }
}
