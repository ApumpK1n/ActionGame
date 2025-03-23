
using UnityHFSM;

internal class PlayerDashState : PlayerMoveState
{
    float AccelerateSpeed = 2f;

    public PlayerDashState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
    : base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.BaseMove, AccelerateSpeed);
    }

}
