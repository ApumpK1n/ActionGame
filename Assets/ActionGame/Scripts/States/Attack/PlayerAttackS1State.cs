

internal class PlayerAttackS1State : PlayerAttackState
{
    public PlayerAttackS1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.Action, AnimationType.Attack, 1f);
    }
}
