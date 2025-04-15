
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    public class FatigueSensor : LocalWorldSensorBase
    {
        public override void Created()
        {

        }

        public override void Update()
        {

        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return references.GetCachedComponent<DataBehavior>().Fatigue;
        }
    }
}
