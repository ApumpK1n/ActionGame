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

        // 间隔
        public ModifierMagnitudeBaseConfig DurationModifier;
        public float BaseDuration;

        public EffectModifier[] Modifiers;
    }
}

