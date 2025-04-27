
using UnityHFSM;

internal class PlayerDashState : PlayerInGroundState
{

    public PlayerDashState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
    : base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.BaseMove, blackboard.CharacterConfig.DashMoveSpeed);
    }

}
