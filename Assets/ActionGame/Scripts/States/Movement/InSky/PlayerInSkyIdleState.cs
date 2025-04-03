

public class PlayerInSkyIdleState : PlayerInSkyState
{
    public PlayerInSkyIdleState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
    : base(blackboard, needsExitTime, isGhostState) { }


    public override void OnEnter()
    {
        blackboard.Player.StopAnimation(PlayerAnimationLayer.LowerBody);
    }
}
