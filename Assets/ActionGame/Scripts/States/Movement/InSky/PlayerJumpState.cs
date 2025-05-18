
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
        blackboard.DownInSkyTime = 0f;
        if (blackboard.Player != null)
        {
            animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.LowerBody, AnimationType.Jump, 1f);
        }
        if (blackboard.CharacterView != null)
        {
            animancerState = blackboard.CharacterView.PlayAnimation(PlayerAnimationLayer.LowerBody, AnimationType.Jump, 1f);
        }
        animancerState.Time = 0f;
        animancerState.SetWeight(1f);
        animancerState.NormalizedEndTime = 0.7f;
        animancerState.Events.OnEnd = OnEnd;
    }

    private void OnEnd()
    {
        animancerState.Events.OnEnd = null;
        animancerState.SetWeight(0f);
        
        if (blackboard.Player != null)
        {
            blackboard.Player.RequestMovementStateChange(MovementStates.InGround);
        }
        if (blackboard.CharacterView != null)
        {
            blackboard.CharacterView.RequestMovementStateChange(MovementStates.InGround);
        }
    }
}
