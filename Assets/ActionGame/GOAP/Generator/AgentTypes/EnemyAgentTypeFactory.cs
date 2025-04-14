
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using CrashKonijn.Goap.ActionGame.Capabilities;

namespace CrashKonijn.Goap.ActionGame.AgentTypes
{
    public class EnemyAgentTypeFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var factory = new AgentTypeBuilder(Define.ScriptEnemyAgent.ToString());

            factory.AddCapability<IdleCapability>();
            //factory.AddCapability<PearCapability>();
            //factory.AddCapability<EatCapability>();

            return factory.Build();
        }
    }
}
