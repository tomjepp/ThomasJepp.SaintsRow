using System;
using System.Runtime.InteropServices;

namespace ThomasJepp.SaintsRow.Saves.SaintsRowIVMod.Sections.Player
{
    [StructLayout(LayoutKind.Explicit, Size = 0x70)]
    public struct SavedPlayerData
    {
        [FieldOffset(0x00)]
        public int Rank;

        [FieldOffset(0x04)]
        public int CashOnHand;

        [FieldOffset(0x08)]
        public int Orbs;

        [FieldOffset(0x0C)]
        public int TotalRespect;

        [FieldOffset(0x10)]
        public int CurrentRespect;

        [FieldOffset(0x14)]
        public int RespectLevel;

        [FieldOffset(0x18)]
        public ushort ReceivedPhonecalls;

        [FieldOffset(0x1A)]
        public bool HasCheated;

        [FieldOffset(0x1C)]
        public float VehicleRepairDiscount;

        [FieldOffset(0x20)]
        public float RespectBonusModifier;

        [FieldOffset(0x24)]
        public bool VehicleCustomizationUnlocked;

        [FieldOffset(0x28)]
        public DifficultyLevelType DifficultyLevel;

        [FieldOffset(0x2C)]
        public CollectiblePersonaSavedInfo CollectibleLineInfo;

        [FieldOffset(0x30)]
        public int NumSecretAreasFound;

        [FieldOffset(0x34)]
        public int NumJumpsFound;

        [FieldOffset(0x38)]
        public float WardenBeatdownDifficultyFactor;

        [FieldOffset(0x3C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public float[] DamageResistFactors;

        [FieldOffset(0x50)]
        public uint HealthRestoreWaitBonusMs;

        [FieldOffset(0x54)]
        public float HealthRestoreRateModifier;

        [FieldOffset(0x58)]
        public float SprintBonus;

        [FieldOffset(0x5C)]
        public bool UnlimitedSprint;

        [FieldOffset(0x60)]
        public float MeleeDamageModifier;

        [FieldOffset(0x64)]
        public float CheatFallDamageModifier;

        [FieldOffset(0x68)]
        public float FirearmAccuracyModifier;

        [FieldOffset(0x6C)]
        public ushort RewardPaymentsAmount;

    }
}
