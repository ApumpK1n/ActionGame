
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    public class IsNearTargetSensor : LocalWorldSensorBase
    {

        public override void Created()
        {

        }

        public override void Update()
        {
            //if (agent != null && references != null)
            //{
            //    Sense(agent, references);
            //}
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return references.GetCachedComponent<DataBehaviour>().IsNear();
        }
    }
}
