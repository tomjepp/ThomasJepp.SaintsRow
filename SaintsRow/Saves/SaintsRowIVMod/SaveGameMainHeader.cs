using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod
{
    /*
     * struct save_game_main_header {
     * // 8 bytes 
     * et_uint32	 m_signature;
     * et_uint32	 m_version;
     * checksum_mem	 m_checksum;
     * };
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x0C)]
    public struct SaveGameMainHeader
    {
        [FieldOffset(0x00)]
        public UInt32 Signature;

        [FieldOffset(0x04)]
        public UInt32 Version;

        [FieldOffset(0x08)]
        public UInt32 Checksum;
    }
}
