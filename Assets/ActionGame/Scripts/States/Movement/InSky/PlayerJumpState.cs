
using Animancer;
using UnityHFSM;
using UnityEngine;

internal class PlayerJumpState : PlayerInSkyState
{

    AnimancerState animancerState;

    public PlayerJumpState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }

    public override void Init() { }

    public override void OnEnter()
    {
        animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.LowerBody, AnimationType.Jump, 1f);
        animancerState.Time = 0f;
        animancerState.SetWeight(1f);
        animancerState.NormalizedEndTime = 1f;
        animancerState.Events.OnEnd = OnEnd;
    }

    private void OnEnd()
    {
        animancerState.Events.OnEnd = null;
        animancerState.SetWeight(0f);
        blackboard.Player.RequestMovementStateChange(MovementStates.InGround);
    }
}
