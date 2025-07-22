using System;

using UnityEngine;

namespace CombatAbilitySystem
{
    /// <summary>
    /// 效果配置
    /// </summary>
    [CreateAssetMenu(menuName = "CombatAbilitySystem/Effect/EffectConfig")]
    public class EffectConfig : ScriptableObject
    {
        [Header("效果")] public EffectModifier[] Modifiers;

        [Header("间隔类型")] public DurationType DurationType;
        [Header("基础持续时间")] public float BaseDuration;
        [Header("持续时间变化")] public ModifierMagnitudeBaseConfig DurationModifier;
        [Header("Tick配置")] public EffectTickPeriod TickPeriod;
    }

    [Serializable]
    public struct EffectTickPeriod
    {
        // Tick间隔
        [Header("Tick间隔")] public float Period;
        // 是否在创建时Tick
        [Header("是否在创建时Tick一次")] public bool ExecuteOnFirstTick;
    }
}

