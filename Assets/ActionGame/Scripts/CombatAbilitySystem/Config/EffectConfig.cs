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
        public DurationType DurationType;

        // 间隔变化
        public ModifierMagnitudeBaseConfig DurationModifier;
        public float BaseDuration;

        public EffectModifier[] Modifiers;

        public EffectTickPeriod TickPeriod;
    }

    [Serializable]
    public struct EffectTickPeriod
    {
        // Tick间隔
        public float Period;
        // 是否在创建时Tick
        public bool ExecuteOnFirstTick;
    }
}

