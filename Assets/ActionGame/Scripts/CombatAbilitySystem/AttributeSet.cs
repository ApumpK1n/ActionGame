using System;
using System.Collections.Generic;

namespace CombatAbilitySystem
{
    /// <summary>
    /// 属性集 存储属性
    /// </summary>
    public class AttributeSet
    {
        private Dictionary<AttributeConfig, AttributeValue> attributeCache;
        private Dictionary<AttributeConfig, AttributeValue> preAttributeCache;

        public AttributeSet(int capacity)
        {
            attributeCache = new Dictionary<AttributeConfig, AttributeValue>(capacity);
            preAttributeCache = new Dictionary<AttributeConfig, AttributeValue>(capacity);
        }

        public void AddAttribute(AttributeConfig attribute)
        {
            if (attributeCache.ContainsKey(attribute))
            {
                return;
            }
            AttributeValue value = new AttributeValue();
            attributeCache.Add(attribute, value);
            preAttributeCache.Add(attribute, value);
        }

        public bool GetAttributeValue(AttributeConfig attribute, out AttributeValue value)
        {
            // We use a cache to store the index of the attribute in the list, so we don't
            // have to iterate through it every time
            if (attributeCache.TryGetValue(attribute, out var attributeValue))
            {
                value = attributeValue;
                return true;
            }


            // No matching attribute found
            value = new AttributeValue();
            attributeCache.Add(attribute, value);
            return false;
        }

        public bool GetPreAttributeValue(AttributeConfig attribute, out AttributeValue value)
        {
            // We use a cache to store the index of the attribute in the list, so we don't
            // have to iterate through it every time
            if (preAttributeCache.TryGetValue(attribute, out var attributeValue))
            {
                value = attributeValue;
                return true;
            }


            // No matching attribute found
            value = new AttributeValue();
            preAttributeCache.Add(attribute, value);
            return false;
        }

        public void InitBaseValue(AttributeConfig attribute, float value)
        {
            GetAttributeValue(attribute, out AttributeValue attributeValue);
            attributeValue.BaseValue = value;

            GetPreAttributeValue(attribute, out AttributeValue preAttributeValue);
            preAttributeValue.BaseValue = value;
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
                case AttributeModifierOperation.Divide:
                    attributeValue.BaseValue /= magnitude;
                    break;
            }
        }
    }

    [Serializable]
    public struct AttributeValue
    {
        public AttributeConfig Attribute;
        public float BaseValue;
        public float CurrentValue;
       // public AttributeModifier Modifier;
    }

}
