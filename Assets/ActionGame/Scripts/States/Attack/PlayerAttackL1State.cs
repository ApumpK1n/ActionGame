

internal class PlayerAttackL1State : PlayerAttackState
{
    public PlayerAttackL1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.L1Attack, 1f);
    }
}
