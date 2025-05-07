

using UnityEngine;

namespace CombatAbilitySystem
{
    /// <summary>
    /// 效果配置
    /// </summary>
    [CreateAssetMenu]
    public class EffectConfig : ScriptableObject
    {
        [Header("Id")] public int Id;
        [Header("描述")] public string Description;
    }
}

