
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    public class EnemyAgentTypeFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var factory = new AgentTypeBuilder(Define.ScriptEnemyAgent.ToString());

            factory.AddCapability<IdleCapability>();
            factory.AddCapability<WanderCapability>();
            //factory.AddCapability<MeleeAttackCapability>();

            return factory.Build();
        }
    }
}
