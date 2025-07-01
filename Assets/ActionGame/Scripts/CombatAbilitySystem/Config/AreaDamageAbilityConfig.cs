

using UnityEngine;

namespace CombatAbilitySystem
{

    [CreateAssetMenu(menuName = "CombatAbilitySystem/AreaDamageAbilityConfig")]
    public class AreaDamageAbilityConfig : AbilityConfig
    {
        [Header("范围类型")]public AreaType AreaType;
        public float RadiusX;
        public float RadiusY;
        public GameObject Prefab;
        public float BaseDamage;
    }

    public enum AreaType
    {
        Circle,
        Rectangle,
    }
}

