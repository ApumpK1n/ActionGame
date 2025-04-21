using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    public class AttackTargetSensor : LocalTargetSensorBase
    {

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var dataComponent = references.GetCachedComponent<DataBehaviour>();
            if (dataComponent.AttackTarget != null)
            {
                return new PositionTarget(dataComponent.AttackTarget.position);
            }

            return new PositionTarget(agent.Transform.position);
        }
    }
}

