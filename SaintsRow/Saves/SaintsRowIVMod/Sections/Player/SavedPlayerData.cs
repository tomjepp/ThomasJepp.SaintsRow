using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod.Sections.Player
{
    /*
     * struct saved_player_data {
     * int	 m_rank; 
     * 
     * // Cash on hand
     * human_cash	 m_cash_on_hand;
     * int	 m_orbs;
     * 
     * int	 m_total_respect;
     * int	 m_current_respect;
     * int	 m_respect_level;
     * 
     * // Cellphone
     * uint16	 m_received_phonecalls; // Received phone calls bitfield
     * 
     * // True if the player has cheated.
     * bool	 m_has_cheated;
     * 
     * float	 m_vehicle_repair_discount;
     * float	 m_respect_bonus_modifier;
     * bool	 m_vehicle_customization_unlocked;
     * 
     * // Difficulty level
     * difficulty_level_type	 m_difficulty_level;
     * 
     * collectible_persona_saved_info	 m_collectible_line_info;
     * 
     * int	 m_num_secret_areas_found;
     * int	 m_num_jumps_found;
     * 
     * float	 m_warden_beatdown_difficulty_factor;
     * float											m_damage_resist_factors[DAMAGE_SOURCE_NUM_TYPES];
     * 
     * 
     * // Player health restore wait bonus
     * uint											m_health_restore_wait_bonus_ms;
     * float											m_health_restore_rate_modifier;
     * 
     * float											m_sprint_bonus;
     * bool											m_unlimited_sprint;
     * float											m_melee_damage_modifier;
     * float											m_cheat_fall_damage_modifier;
     * float											m_firearm_accuracy_modifier;
     * uint16										m_reward_payments_amount;
     * };
     */

    [StructLayout(LayoutKind.Explicit, Size = 0x1A)]
    public struct SavedPlayerData
    {
        [FieldOffset(0x00)]
        public Int32 Rank;

        [FieldOffset(0x04)]
        public Int32 CashOnHand;
        [FieldOffset(0x08)]
        public Int32 Orbs;

        [FieldOffset(0x0C)]
        public Int32 TotalRespect;
        [FieldOffset(0x10)]
        public Int32 CurrentRespect;
        [FieldOffset(0x14)]
        public Int32 RespectLevel;

        [FieldOffset(0x18)]
        public UInt16 ReceivedPhonecalls;

        //[FieldOffset(0x1A)]
        //[MarshalAs(UnmanagedType.U1)]
        //public bool HasCheated;
    }
}
