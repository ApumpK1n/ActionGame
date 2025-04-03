
using Animancer;
using UnityHFSM;
using UnityEngine;

internal class PlayerDownState : PlayerInSkyState
{

    AnimancerState animancerState;

    public PlayerDownState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }

    public override void Init() { }

    public override void OnEnter()
    {
        base.OnEnter();
        blackboard.DownDistance = 0f;
        blackboard.DownInSkyTime = 0f;


        //animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.LowerBody, AnimationType.Down, 1f);
        //animancerState.Time = 0f;
        //animancerState.SetWeight(1f);
    }
}
