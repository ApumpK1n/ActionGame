

public class PlayerIdleState : PlayerInGroundState
{
    public PlayerIdleState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }


    public override void OnEnter()
    {
        if (blackboard.Player != null)
        {
            blackboard.Player.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.Idle, 1f);
        }

        if (blackboard.CharacterView != null)
        {
            blackboard.CharacterView.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.Idle, 1f);
        }
    }
}
