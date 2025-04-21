

using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    public class MeleeAttackCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(MeleeAttackCapability).ToString());

            builder.AddGoal<AttackSucceedGoal>()
                .AddCondition<AttackSucceed>(Comparison.GreaterThanOrEqual, 1); 

            builder.AddAction<MeleeAttackAction>()
                .AddEffect<AttackSucceed>(EffectType.Increase)
                .SetRequiresTarget(false);

            builder.AddAction<MoveToTargetAction>();
            //builder.AddTargetSensor<>
            // builder.AddMultiSensor<EnemyStateSensor>();

            return builder.Build();
        }
    }
}
