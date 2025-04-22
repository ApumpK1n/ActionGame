using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    [GoapId("MoveToTarget-8d14d6f1-a3cb-475b-9de5-5393d9bf34a5")]
    public class MoveToTargetAction : GoapActionBase<MoveToTargetAction.Data>
    {
        // This method is called when the action is created
        // This method is optional and can be removed
        public override void Created()
        {
        }

        // This method is called every frame before the action is performed
        // If this method returns false, the action will be stopped
        // This method is optional and can be removed
        public override bool IsValid(IActionReceiver agent, Data data)
        {
            return true;
        }

        // This method is called when the action is started
        // This method is optional and can be removed
        public override void Start(IMonoAgent agent, Data data)
        {
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation((data.Target.Position - agent.transform.position).normalized), data.DataComponent.EnemyConfig.MoveTurnSpeed);
            data.AnimationComponent.Play(EnemyAnimationLayer.Base, AnimationType.BaseMove);

        }

        // This method is called once before the action is performed
        // This method is optional and can be removed
        public override void BeforePerform(IMonoAgent agent, Data data)
        {
        }

        // This method is called every frame while the action is running
        // This method is required
        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            float distance = Vector3.Distance(agent.transform.position, data.Target.Position);

            if (!data.DataComponent.IsNearAttackTarget()) return ActionRunState.Continue;
            return ActionRunState.Completed;
        }

        // This method is called when the action is completed
        // This method is optional and can be removed
        public override void Complete(IMonoAgent agent, Data data)
        {
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
            public AnimationComponent AnimationComponent { get; set; }
            [GetComponent]
            public DataBehaviour DataComponent { get; set; }
        }
    }
}
