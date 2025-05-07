

using System.Collections.Generic;
using UnityEngine;

namespace CombatAbilitySystem
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("技能Id")] public int Id;
        [Header("描述")] public string Description;
        [Header("冷却时间s")] public int Cooldown;
        [Header("施法前摇")] public float CastPoint;
        [Header("能力效果")] public List<EffectConfig> Effects;
    }
}


