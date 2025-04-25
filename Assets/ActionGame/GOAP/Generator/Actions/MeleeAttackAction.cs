using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using Animancer;
using static CrashKonijn.Goap.ActionGame.WanderAction;

namespace CrashKonijn.Goap.ActionGame
{
    [GoapId("MeleeAttack-1bf08fce-27d0-48d9-87bf-45db9a5a964f")]
    public class MeleeAttackAction : GoapActionBase<MeleeAttackAction.Data>
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
            AnimancerState state = data.AnimationComponent.Play(EnemyAnimationLayer.Base, AnimationType.L1Attack);
            data.Timer = ActionRunState.Wait(state.Length);
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
            if (data.Timer.IsRunning())
                return data.Timer;

            return ActionRunState.Completed;
        }

        // This method is called when the action is completed
        // This method is optional and can be removed
        public override void Complete(IMonoAgent agent, Data data)
        {
            AnimancerState state = data.AnimationComponent.Play(EnemyAnimationLayer.Base, AnimationType.Idle);
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

            public IActionRunState Timer { get; set; }
        }
    }
}
