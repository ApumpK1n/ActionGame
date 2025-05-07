

namespace CombatAbilitySystem
{
    /// <summary>
    /// 技能效果 修改属性
    /// </summary>
    public class EffectExecutor : ITick
    {
        // 效果配置
        public EffectConfig EffectConfig { get; private set; }

        public float TotalDuration;
        public float RemainingDuration;

        public EffectExecutor(EffectConfig config)
        {
            this.EffectConfig = config;
        }

        public static EffectExecutor Create(EffectConfig effectConfig)
        {
            return new EffectExecutor(effectConfig);
        }

        public void Tick(float dt)
        {

        }
    }

}
