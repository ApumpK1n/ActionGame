
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    public class WanderCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(WanderCapability).ToString());

            builder.AddGoal<WanderGoal>()
                .AddCondition<Fatigue>(Comparison.GreaterThan, 0);

            builder.AddAction<WanderAction>()
                //.AddEffect<IsWander>(EffectType.Increase) // 是否在巡逻
                .AddCondition<Fatigue>(Comparison.SmallerThanOrEqual, 10)
                .AddEffect<Fatigue>(EffectType.Increase)
                .SetTarget<WanderTarget>()
                .SetProperties(new WanderAction.Props
                {
                    minTimer = 1f,
                    maxTimer = 2f
                });

            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            //builder.AddMultiSensor<EnemyStateSensor>();

            return builder.Build();
        }
    }
}
