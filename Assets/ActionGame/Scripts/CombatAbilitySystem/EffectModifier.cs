

using System;

namespace CombatAbilitySystem
{
    [Serializable]
    public struct EffectModifier
    {
        public AttributeConfig Attribute;
        public AttributeModifierOperation ModifierOperation;
        public float BaseValue;     //效果基础数值
        public ModifierMagnitudeBaseConfig ModifierMagnitude;
    }
}
