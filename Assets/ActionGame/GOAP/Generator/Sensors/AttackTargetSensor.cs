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


            Vector3 targetPosition = GetPosition(agent, references);
            if (target is PositionTarget positionTarget)
            {
                return positionTarget.SetPosition(targetPosition);
            }

            return new PositionTarget(targetPosition);
        }

        private Vector3 GetPosition(IActionReceiver agent, IComponentReference references)
        {
            var dataComponent = references.GetCachedComponent<DataBehaviour>();
            if (dataComponent.AttackTarget != null)
            {
                return dataComponent.AttackTarget.position;
            }

            if (agent != null)
            {
                return agent.Transform.position;
            }

            return Vector3.zero;
        }
    }
}

