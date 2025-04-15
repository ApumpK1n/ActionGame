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
            var wait = Random.Range(this.Properties.minTimer, this.Properties.maxTimer);

            data.Timer = ActionRunState.Wait(wait);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            Debug.Log("Perform");
            data.DataBehavior.Fatigue += context.DeltaTime *10f;
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
            public DataBehavior DataBehavior { get; set; }
        }
    }
}
