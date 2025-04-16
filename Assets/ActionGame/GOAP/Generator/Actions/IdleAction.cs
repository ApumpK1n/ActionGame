using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Docs.GettingStarted.Behaviours;
using CrashKonijn.Goap.Runtime;
using UnityEngine;


namespace CrashKonijn.Goap.ActionGame
{
    [GoapId("Idle-864856f6-54f6-45a0-b275-f9957d6ab36a")]
    public class IdleAction : GoapActionBase<IdleAction.Data, IdleAction.Props>
    {

        // This method is called when the action is started
        // This method is optional and can be removed
        public override void Start(IMonoAgent agent, Data data)
        {
            var wait = UnityEngine.Random.Range(this.Properties.minTimer, this.Properties.maxTimer);

            data.Timer = new IdleActionRunState(wait, false, data.DataBehavior);

            data.AnimationComponent.Play(EnemyAnimationLayer.Base, AnimationType.Idle);
        }


        // This method is called every frame while the action is running
        // This method is required
        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Timer.IsRunning())
                return data.Timer;
            return ActionRunState.Completed;
        }

        // This method is called when the action is completed
        // This method is optional and can be removed
        public override void Complete(IMonoAgent agent, Data data)
        {
            data.DataBehavior.Fatigue -= 10f;
        }

        // This method is called when the action is stopped
        // This method is optional and can be removed
        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        // This method is called when the action is completed or stopped
        // This method is optional and can be removed
        public override void End(IMonoAgent agent, Data data)
        {
        }

        // The action class itself must be stateless!
        // All data should be stored in the data class
        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            [GetComponent]
            public DataBehaviour DataBehavior { get; set; }

            public IActionRunState Timer { get; set; }


            [GetComponent]
            public AnimationComponent AnimationComponent { get; set; }
        }

        [Serializable]
        public class Props : IActionProperties
        {
            public float minTimer;
            public float maxTimer;
        }

        public class IdleActionRunState : ActionRunState
        {
            private readonly bool mayResolve;

            private float time;
            private DataBehaviour dataBehavior;

            public IdleActionRunState(float time, bool mayResolve, DataBehaviour dataBehavior)
            {
                this.time = time;
                this.mayResolve = mayResolve;

                this.dataBehavior = dataBehavior;
            }

            public override void Update(IAgent agent, IActionContext context)
            {
                time -= context.DeltaTime;
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
