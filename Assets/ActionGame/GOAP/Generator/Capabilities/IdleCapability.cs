

using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    /// <summary>
    ///  休息能力
    /// </summary>
    public class IdleCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(IdleCapability).ToString());

            builder.AddGoal<IdleGoal>()
                .SetBaseCost(10f)
                .AddCondition<Fatigue>(Comparison.SmallerThanOrEqual, 0);

            builder.AddAction<IdleAction>()
                .AddCondition<Fatigue>(Comparison.GreaterThanOrEqual, 10)
                .AddEffect<Fatigue>(EffectType.Decrease)
                .SetProperties(new IdleAction.Props
                {
                    minTimer = 1f,
                    maxTimer = 2f
                })
                .SetRequiresTarget(false);

            // World Sensor
            builder.AddWorldSensor<FatigueSensor>()
                .SetKey<Fatigue>();

           // builder.AddMultiSensor<EnemyStateSensor>();

            return builder.Build();
        }
    }
}
