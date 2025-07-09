

namespace CombatAbilitySystem
{
    /// <summary>
    /// 技能效果 修改属性
    /// </summary>
    public class EffectExecutor : ITick
    {
        // 效果配置
        public EffectConfig EffectConfig { get; private set; }

        public float TotalDuration { get; private set; }// 总时长
        public float RemainingDuration { get; private set; } // 剩余时间
        public float TimeUntilPeriodTick { get; private set; } // 数值Tick计时器
        public bool CanPeriodTick { get; private set; }

        public float Level { get; private set; }    // 等级 用于效果强度计算
        public bool IsEnd => RemainingDuration <= 0;

        public AbilitySystemComponent ActorSource { get; private set; }
        public AbilityComponent AbilitySource { get; private set; }

        public EffectExecutor(AbilitySystemComponent actorSource, AbilityComponent abilitySource, EffectConfig config, float level=1)
        {
            this.EffectConfig = config;
            this.AbilitySource = abilitySource;
            this.ActorSource = actorSource;

            if (this.EffectConfig.DurationModifier)
            {
                this.TotalDuration = this.EffectConfig.DurationModifier.CalculateMagnitude(this) * this.EffectConfig.BaseDuration;
                this.RemainingDuration = this.TotalDuration;
            }
            if (config.TickPeriod.ExecuteOnFirstTick)
            {
                TimeUntilPeriodTick = 0;
                CanPeriodTick = true;
            }
            else
            {
                TimeUntilPeriodTick = config.TickPeriod.Period;
                CanPeriodTick = false;
            }
        }

        public static EffectExecutor Create(AbilitySystemComponent actorSource, AbilityComponent abilitySource, EffectConfig effectConfig, float level)
        {
            return new EffectExecutor(actorSource, abilitySource, effectConfig, level);
        }

        public void SetTotalDuration(float totalDuration)
        {
            this.TotalDuration = totalDuration;
        }


        public void SetRemainingDuration(float remainingDuration)
        {
            RemainingDuration = remainingDuration;
        }

        public void Tick(float dt)
        {
            RemainingDuration -= dt;
            TimeUntilPeriodTick -= dt;
            CanPeriodTick = false;
            if (TimeUntilPeriodTick <= 0)
            {
                TimeUntilPeriodTick = this.EffectConfig.TickPeriod.Period;
                CanPeriodTick = true;
            }
        }
    }

}
