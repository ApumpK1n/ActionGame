

using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    public class IdleCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(IdleCapability).ToString());

            builder.AddGoal<IdleGoal>()
                .AddCondition<IsIdle>(Comparison.GreaterThanOrEqual, 1)
                .SetBaseCost(2);

            builder.AddAction<IdleAction>()
                .AddEffect<IsIdle>(EffectType.Increase)
                .AddCondition<Fatigue>(Comparison.GreaterThanOrEqual, 10)
                .SetRequiresTarget(false)
                .SetBaseCost(10);

            // World Sensor
            builder.AddWorldSensor<FatigueSensor>()
                .SetKey<Fatigue>();

            return builder.Build();
        }
    }
}
