using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod
{
    /*
     * struct save_game_section {
     * et_uint32	 m_section_id;
     * et_uint32	 m_version;
     * et_uint32	 m_size;
     * };
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x0C)]
    public struct SaveGameSectionHeader
    {
        [FieldOffset(0x00)]
        public SectionId SectionId;

        [FieldOffset(0x04)]
        public UInt32 Version;

        [FieldOffset(0x08)]
        public UInt32 Size;
    }
}
