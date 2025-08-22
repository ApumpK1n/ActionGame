

namespace CombatAbilitySystem
{
    /// <summary>
    /// 有持续时间的效果容器, 用于存储持续时间结束后 应该还原的数值
    /// </summary>
    public struct EffectExecutorContainer
    {
        public EffectExecutor executor;
        public ModifierContainer[] modifiers;

    }

    public struct ModifierContainer
    {
        public AttributeConfig Attribute;
        public AttributeModifier Modifier;
    }
}
