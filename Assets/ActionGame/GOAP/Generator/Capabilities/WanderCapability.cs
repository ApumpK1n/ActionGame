
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    /// <summary>
    /// 巡逻能力 在区域范围内进行随机移动
    /// </summary>
    public class WanderCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(WanderCapability).ToString());

            builder.AddGoal<WanderGoal>()
                .SetBaseCost(10f)
                .AddCondition<IsWander>(Comparison.GreaterThanOrEqual, 1);

            builder.AddAction<WanderAction>()
                //.AddEffect<IsWander>(EffectType.Increase) // 是否在巡逻
                .AddCondition<Fatigue>(Comparison.SmallerThanOrEqual, 10)
                .AddEffect<Fatigue>(EffectType.Increase)
                .AddEffect<IsWander>(EffectType.Increase)
                .SetTarget<WanderTarget>()
                .SetProperties(new WanderAction.Props
                {
                    minTimer = 1f,
                    maxTimer = 2f
                });

            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();
            builder.AddWorldSensor<IsWanderSensor>().SetKey<IsWander>();

            //builder.AddMultiSensor<EnemyStateSensor>();

            return builder.Build();
        }
    }
}
