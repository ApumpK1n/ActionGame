using CrashKonijn.Docs.GettingStarted.Capabilities;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Docs.GettingStarted.AgentTypes
{
    public class DemoAgentTypeFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var factory = new AgentTypeBuilder("ScriptDemoAgent");

            factory.AddCapability<IdleCapabilityFactory>();
            factory.AddCapability<PearCapability>();
            factory.AddCapability<EatCapability>();

            return factory.Build();
        }
    }
}
