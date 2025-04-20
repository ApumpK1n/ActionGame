
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    public class EnemyStateSensor : MultiSensorBase
    {
        public EnemyStateSensor()
        {
            this.AddLocalWorldSensor<IsIdle>(this.SenseIsIdle);
            this.AddLocalWorldSensor<IsWander>(SenseIsWander);
        }

        public override void Created()
        {

        }

        public override void Update()
        {
        }

        private SenseValue SenseIsIdle(IActionReceiver agent, IComponentReference references)
        {
            return references.GetCachedComponent<DataBehaviour>().IsIdle == true;
        }

        private SenseValue SenseIsWander(IActionReceiver agent, IComponentReference references)
        {
            return references.GetCachedComponent<DataBehaviour>().IsWander == true;
        }
    }
}
