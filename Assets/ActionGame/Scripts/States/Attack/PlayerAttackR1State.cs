

internal class PlayerAttackR1State : PlayerAttackState
{
    public PlayerAttackR1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.R1Attack, 1f);
    }
}
