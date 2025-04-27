using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    public class WanderTargetSensor : LocalTargetSensorBase
    {

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var random = this.GetRandomPosition(references);

            // If we already have a target, update it with the new position
            if (target is PositionTarget positionTarget)
                return positionTarget.SetPosition(random);

            return new PositionTarget(random);
        }

        private Vector3 GetRandomPosition(IComponentReference references)
        {
            Transform belongArea = references.GetCachedComponent<DataBehaviour>().BelongArea;

            Vector2 random = Vector2.zero;
            random.x = Random.Range(0f, 20f);
            random.y = Random.Range(0f, 20f);
            var position = belongArea.position + new Vector3(random.x, 1f, random.y);

            return position;
            //if (position.x > -Bounds.x && position.x < Bounds.x && position.z > -Bounds.y && position.z < Bounds.y)
            //    return position;

            //return this.GetRandomPosition(agent);
        }
    }
}

