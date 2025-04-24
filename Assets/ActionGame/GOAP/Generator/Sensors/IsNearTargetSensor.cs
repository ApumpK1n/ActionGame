
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

        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            Debug.Log("references.GetCachedComponent<DataBehaviour>().IsNearAttackTarget()" + references.GetCachedComponent<DataBehaviour>().IsNearAttackTarget());
            return references.GetCachedComponent<DataBehaviour>().IsNearAttackTarget();
        }
    }
}
