using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using CrashKonijn.Agent.Runtime;

namespace CrashKonijn.Goap.ActionGame
{
    [GoapId("Wander-43cec5ac-b1d5-47ec-9620-905a5aa70876")]
    public class WanderAction : GoapActionBase<WanderAction.Data, WanderAction.Props>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            //var wait = Random.Range(this.Properties.minTimer, this.Properties.maxTimer);

            float distance = Vector3.Distance(data.Target.Position, agent.gameObject.transform.position);
            float t = distance / 60;

            Debug.Log("distance:" + distance);
            data.Timer = new WanderActionRunState(t, false, data.DataBehavior);
            data.AnimationComponent.Play(EnemyAnimationLayer.Base, AnimationType.BaseMove);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Timer.IsRunning())
                return data.Timer;

            return ActionRunState.Completed;
        }

        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
            Debug.Log("CompleteWanderAction");
        }

        [Serializable]
        public class Props : IActionProperties
        {
            public float minTimer;
            public float maxTimer;
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IActionRunState Timer { get; set; }

            [GetComponent]
            public DataBehaviour DataBehavior { get; set; }

            [GetComponent]
            public AnimationComponent AnimationComponent { get; set; }
        }

        public class WanderActionRunState : ActionRunState
        {
            private readonly bool mayResolve;

            private float time;
            private DataBehaviour dataBehavior;

            public WanderActionRunState(float time, bool mayResolve, DataBehaviour dataBehavior)
            {
                this.time = time;
                this.mayResolve = mayResolve;

                this.dataBehavior = dataBehavior;
            }

            public override void Update(IAgent agent, IActionContext context)
            {
                time -= context.DeltaTime;
                dataBehavior.Fatigue += context.DeltaTime * 50f;

                Debug.Log("Update");
            }

            public override bool ShouldStop(IAgent agent)
            {
                return false;
            }

            public override bool ShouldPerform(IAgent agent)
            {
                return time <= 0f;
            }

            public override bool IsCompleted(IAgent agent)
            {
                return false;
            }

            public override bool MayResolve(IAgent agent)
            {
                return mayResolve;
            }

            public override bool IsRunning()
            {
                return time > 0f;
            }
        }
    }
}
