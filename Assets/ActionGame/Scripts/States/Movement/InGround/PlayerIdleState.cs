

public class PlayerIdleState : PlayerInGroundState
{
    public PlayerIdleState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }


    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.Idle, 1f);
    }
}
