

using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    /// <summary>
    /// 近战攻击的能力 先走到目的地 再进行攻击
    /// </summary>
    public class GuardCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder(typeof(GuardCapability).ToString());

            builder.AddGoal<GuardGoal>()
                .AddCondition<AttackSucceed>(Comparison.GreaterThanOrEqual, 1); 

            // 近战攻击
            builder.AddAction<MeleeAttackAction>()
                .AddCondition<IsNearTarget>(Comparison.GreaterThanOrEqual, 1)
                .AddEffect<AttackSucceed>(EffectType.Increase)
                .SetTarget<AttackTarget>();

            // 移动到目的地
            builder.AddAction<MoveToTargetAction>()
                .AddCondition<IsFindingAttackTarget>(Comparison.GreaterThanOrEqual, 1)
                .AddEffect<IsNearTarget>(EffectType.Increase)
                .SetTarget<AttackTarget>();
            builder.AddTargetSensor<AttackTargetSensor>().SetTarget<AttackTarget>();

            builder.AddWorldSensor<IsFindingAttackTargetSensor>().SetKey<IsFindingAttackTarget>();
            builder.AddWorldSensor<IsNearTargetSensor>().SetKey<IsNearTarget>();

            return builder.Build();
        }
    }
}
