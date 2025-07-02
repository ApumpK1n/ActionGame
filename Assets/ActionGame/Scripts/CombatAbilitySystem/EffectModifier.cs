

using System;

namespace CombatAbilitySystem
{
    [Serializable]
    public struct EffectModifier
    {
        public AttributeConfig Attribute;
        public AttributeModifierOperation ModifierOperation;
        public float BaseValue;
        public ModifierMagnitudeBaseConfig ModifierMagnitude;
    }
}
