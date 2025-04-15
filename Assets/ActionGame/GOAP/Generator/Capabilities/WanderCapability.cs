
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
                .AddCondition<IsWander>(Comparison.GreaterThan, 0)
                .SetBaseCost(1);

            builder.AddAction<WanderAction>()
                .AddEffect<IsWander>(EffectType.Increase) // 是否在巡逻
                .AddEffect<Fatigue>(EffectType.Increase)
                .SetTarget<WanderTarget>()
                .SetBaseCost(1)
                .SetProperties(new WanderAction.Props
                {
                    minTimer = 1f,
                    maxTimer = 2f
                });

            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            return builder.Build();
        }
    }
}
