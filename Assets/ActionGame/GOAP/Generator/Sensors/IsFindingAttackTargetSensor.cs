
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    public class IsFindingAttackTargetSensor : LocalWorldSensorBase
    {
        public override void Created()
        {

        }

        public override void Update()
        {

        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            //Debug.Log($"IsFindingAttackTargetSensor{references.GetCachedComponent<DataBehaviour>().AttackTarget}");
            return references.GetCachedComponent<DataBehaviour>().AttackTarget != null;
        }
    }
}
