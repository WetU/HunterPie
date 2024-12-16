﻿using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using System.Runtime.CompilerServices;
using QuestType = HunterPie.Core.Game.Entity.Game.Quest.QuestType;
using QuestTypeRise = HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums.QuestType;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

public static class MHRiseUtils
{
    private const double MAX_DEFAULT_STAMINA = 1500.0;
    private const double FOOD_BONUS_STAMINA = 3000.0;
    private const double MAX_DEFAULT_HEALTH = 100.0;
    private const double FOOD_BONUS_HEALTH = 50.0;
    private const double PETALACE_STAMINA_MULTIPLIER = 30.0;
    private const float TIMER_MULTIPLIER = 60.0f;

    public static MonsterPartDefinition QurioPartDefinition =
        new MonsterPartDefinition { String = "PART_QURIO_THRESHOLD" };

    public static Weapon ToWeaponId(this int self)
    {
        return self switch
        {
            0 => Weapon.Greatsword,
            1 => Weapon.SwitchAxe,
            2 => Weapon.Longsword,
            3 => Weapon.LightBowgun,
            4 => Weapon.HeavyBowgun,
            5 => Weapon.Hammer,
            6 => Weapon.GunLance,
            7 => Weapon.Lance,
            8 => Weapon.SwordAndShield,
            9 => Weapon.DualBlades,
            10 => Weapon.HuntingHorn,
            11 => Weapon.ChargeBlade,
            12 => Weapon.InsectGlaive,
            13 => Weapon.Bow,
            _ => Weapon.None,
        };
    }

    public static bool IsInQuest(this int self) => self == 2;
    public static bool IsQuestFinished(this int self) => self > 2;
    public static bool IsTrainingRoom(this int self) => self == 5;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CalculateMaxPlayerHealth(this MHRPetalaceStatsStructure stats) =>
        stats.HealthUp + MAX_DEFAULT_HEALTH + FOOD_BONUS_HEALTH;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CalculateMaxPlayerStamina(this MHRPetalaceStatsStructure stats) =>
        (stats.StaminaUp * PETALACE_STAMINA_MULTIPLIER) + MAX_DEFAULT_STAMINA + FOOD_BONUS_STAMINA;

    public static T[] ReadArray<T>(this IMemory memory, long address) where T : struct
    {
        uint arraySize = memory.Read<uint>(address + 0x1C);
        return memory.Read<T>(address + 0x20, arraySize);
    }

    public static T[] ReadArrayOfPtrs<T>(this IMemory memory, long address) where T : struct
    {
        return memory.ReadArray<long>(address)
            .Select(memory.Read<T>)
            .ToArray();
    }

    public static T[] ReadArrayOfPtrsSafe<T>(this IMemory memory, long address, uint size) where T : struct
    {
        return memory.ReadArraySafe<long>(address, size)
            .Select(memory.Read<T>)
            .ToArray();
    }

    public static T[] ReadArraySafe<T>(this IMemory memory, long address, uint size) where T : struct
    {
        uint arraySize = memory.Read<uint>(address + 0x1C);
        arraySize = Math.Min(size, arraySize);
        return memory.Read<T>(address + 0x20, arraySize);
    }

    public static IReadOnlyCollection<T> ReadListOfPtrsSafe<T>(this IMemory memory, long address, uint size)
        where T : struct
    {
        return memory.ReadListSafe<long>(address, size)
            .Select(memory.Read<T>)
            .ToArray();
    }

    public static IReadOnlyCollection<T> ReadListSafe<T>(this IMemory memory, long address, uint size) where T : struct
    {
        uint listSize = memory.Read<uint>(address + 0x18);
        listSize = Math.Min(listSize, size);
        return memory.Read<T>(address + 0x20, listSize);
    }

    public static IReadOnlyCollection<T> ReadList<T>(this IMemory memory, long address) where T : struct
    {
        uint listSize = memory.Read<uint>(address + 0x18);
        return memory.Read<T>(address + 0x20, listSize);
    }

    public static WirebugType ToType(this MHRWirebugCountStructure count, int index)
    {
        int obtainableWirebugs = count.Default + count.Environment;

        if (index < count.Default)
            return WirebugType.Default;
        else if (count.AnyEnvironment() && index == count.Default)
            return WirebugType.Environment;
        else if (count.AnySkill() && index == obtainableWirebugs)
            return WirebugType.Skill;
        else
            return WirebugType.None;
    }

    public static KinsectBuff ToBuff(this KinsectExtract extract) =>
        extract switch
        {
            KinsectExtract.Attack => KinsectBuff.Attack,
            KinsectExtract.Speed => KinsectBuff.Speed,
            KinsectExtract.Defense => KinsectBuff.Defense,
            _ => KinsectBuff.None,
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToAbnormalitySeconds(this float timer) => timer / TIMER_MULTIPLIER;

    public static PhialChargeLevel ToPhialChargeLevel(this float buildup)
    {
        return buildup switch
        {
            >= 72.0f => PhialChargeLevel.Overcharged,
            >= 46.0f => PhialChargeLevel.Red,
            >= 30.0f => PhialChargeLevel.Yellow,
            _ => PhialChargeLevel.None,
        };
    }

    public static QuestStatus ToQuestStatus(this QuestState state)
    {
        return state switch
        {
            QuestState.InQuest => QuestStatus.InProgress,
            QuestState.Success => QuestStatus.Success,
            QuestState.Failed => QuestStatus.Fail,
            QuestState.Reset or QuestState.Returning => QuestStatus.Quit,
            _ => QuestStatus.None
        };
    }

    public static QuestLevel ToQuestLevel(this int level)
    {
        return level switch
        {
            2 => QuestLevel.HighRank,
            3 => QuestLevel.MasterRank,
            _ => QuestLevel.LowRank
        };
    }

    public static bool IsQuestOver(this QuestState state)
    {
        return state is QuestState.Failed or QuestState.Reset or QuestState.Returning;
    }

    public static QuestType? ToQuestType(this QuestTypeRise type)
    {
        return type switch
        {
            QuestTypeRise.Normal => QuestType.Hunt,
            QuestTypeRise.Kill => QuestType.Slay,
            QuestTypeRise.Capture => QuestType.Capture,
            QuestTypeRise.Gather => QuestType.Delivery,
            QuestTypeRise.Arena or
            QuestTypeRise.Boss or
            QuestTypeRise.Special => QuestType.Special,
            _ => null
        };
    }
}