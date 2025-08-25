using Main.Runtime.Core.StatSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace KHJ.Shared
{
    public enum ModifierType
    {
        Stat,
        Shield
    }

    public enum ModifierStatType
    {
        IncreaseValue,
        DecreaseValue,
        IncreaseValuePercent,
        DecreaseValuePercent,
    }

    public enum ModifierShieldType
    {
        Flat,
        MaxHpPercent
    }

    [Serializable]
    public struct ModifierStat
    {
        [HideIf(
             "@this.modifierType == ModifierType.Shield"), BoxGroup("ModifierStat")]
        public StatSO modifierStat;

        [HideIf(
             "@this.modifierType == ModifierType.Shield"), BoxGroup("ModifierStat")]
        public ModifierStatType modifierStatType;

        [HideIf(
             "@this.modifierType == ModifierType.Stat"), BoxGroup("ModifierStat")]
        public ModifierShieldType modifierShieldType;

        public float modifierValue;

        public ModifierType modifierType;
    }

    public class ModifierStatInfo
    {
        public ModifierStat[] ModifierStats;
        [HideInInspector] public Action<ModifierStatInfo> AddModifierEvent;
        [HideInInspector] public Action<ModifierStatInfo> RemoveModifierEvent;
    }

    public interface IModifierStatPassive
    {
        public ModifierStatInfo ModifierStatInfo { get; set; }
    }

    public interface IEffectPoolPassive
    {
        public PoolTypeSO PoolType { get; set; }
    }
}