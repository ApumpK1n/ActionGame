using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatAbilitySystem
{
    /// <summary>
    /// 属性集 存储属性
    /// </summary>
    public class AttributeSet
    {
        private Dictionary<AttributeConfig, AttributeValue> attributeCache;

        public AttributeSet(int capacity)
        {
            attributeCache = new Dictionary<AttributeConfig, AttributeValue>(capacity);
        }

        public void AddAttribute(AttributeConfig attribute)
        {
            if (attributeCache.ContainsKey(attribute))
            {
                return;
            }
            AttributeValue value = new AttributeValue();
            ResetAttributeModify(value.Modifier);
            attributeCache.Add(attribute, value);
        }

        public bool GetAttributeValue(AttributeConfig attribute, out AttributeValue value)
        {
            // We use a cache to store the index of the attribute in the list, so we don't
            // have to iterate through it every time
            if (attributeCache.TryGetValue(attribute, out value))
            {
                return true;
            }


            // No matching attribute found
            value = new AttributeValue();
            attributeCache.Add(attribute, value);
            return false;
        }

        public void InitBaseValue(AttributeConfig attribute, float value)
        {
            GetAttributeValue(attribute, out AttributeValue attributeValue);
            attributeValue.BaseValue = value;
            attributeValue.CurrentValue = value;
        }

        public void SetAttributeBaseValueModify(EffectModifier modifier, float magnitude)
        {
            GetAttributeValue(modifier.Attribute, out var attributeValue);

            switch (modifier.ModifierOperation)
            {
                case AttributeModifierOperation.Add:
                    attributeValue.BaseValue += magnitude;
                    break;
                case AttributeModifierOperation.Multiply:
                    attributeValue.BaseValue *= magnitude;
                    break;
                case AttributeModifierOperation.Override:
                    attributeValue.BaseValue = magnitude;
                    break;
            }
            attributeValue.CurrentValue = attributeValue.BaseValue; // 同步覆盖当前值
        }

        public void UpdateAttributeModify(AttributeConfig attribute, AttributeModifier modifier)
        {
            GetAttributeValue(attribute, out var attributeValue);

            attributeValue.Modifier = attributeValue.Modifier.Combine(modifier);
        }

        public void ResetAttributeModifiers()
        {
            foreach (var keyValue in attributeCache)
            {
                var attributeValue = keyValue.Value;
                ResetAttributeModify(attributeValue.Modifier);
            }
        }

        private void ResetAttributeModify(AttributeModifier attributeModifier)
        {
            attributeModifier.Add = 0f;
            attributeModifier.Multiply = 0f;
            attributeModifier.Override = float.NaN;
        }

        public void CalculateCurrentAttributeValue(AttributeConfig attributeConfig)
        {
            GetAttributeValue(attributeConfig, out var attributeValue);
            attributeValue.CurrentValue = (attributeValue.BaseValue + attributeValue.Modifier.Add) * (attributeValue.Modifier.Multiply + 1);

            if (attributeValue.Modifier.Override != float.NaN)
            {
                attributeValue.CurrentValue = attributeValue.Modifier.Override;
            }
        }

        public float GetCurrentValue(AttributeConfig attributeConfig)
        {
            GetAttributeValue(attributeConfig, out var attributeValue);
            return attributeValue.CurrentValue;
        }
    }

    [Serializable]
    public class AttributeValue
    {
        public AttributeConfig Attribute;
        public float BaseValue;    // 永久值
        public float CurrentValue; // 当前实际数值
        public AttributeModifier Modifier;

        public AttributeValue()
        {
            Modifier = new AttributeModifier();
        }
    }

    [Serializable]
    public class AttributeModifier
    {
        public float Add;
        public float Multiply;
        public float Override;

        public AttributeModifier Combine(AttributeModifier other)
        {
            other.Add += Add;
            other.Multiply += Multiply;
            other.Override = Override;
            return other;
        }
    }
}
