

using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    /// <summary>
    /// 近战攻击的能力 先走到目的地 再进行攻击
    /// </summary>
    public class MeleeAttackCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(MeleeAttackCapability).ToString());

            builder.AddGoal<AttackSucceedGoal>()
                .AddCondition<AttackSucceed>(Comparison.GreaterThanOrEqual, 1); 

            builder.AddAction<MeleeAttackAction>()
                .AddCondition<IsNearTarget>(Comparison.GreaterThanOrEqual, 1)
                .AddEffect<AttackSucceed>(EffectType.Increase)
                .SetTarget<AttackTarget>();

            builder.AddAction<MoveToTargetAction>()
                .AddCondition<IsFindingAttackTarget>(Comparison.GreaterThanOrEqual, 1)
                .AddEffect<IsNearTarget>(EffectType.Increase)
                .SetTarget<AttackTarget>();
            builder.AddTargetSensor<AttackTargetSensor>().SetTarget<AttackTarget>();
            // builder.AddMultiSensor<EnemyStateSensor>();
            builder.AddWorldSensor<IsFindingAttackTargetSensor>().SetKey<IsFindingAttackTarget>();
            builder.AddWorldSensor<IsNearTargetSensor>().SetKey<IsNearTarget>();

            return builder.Build();
        }
    }
}
