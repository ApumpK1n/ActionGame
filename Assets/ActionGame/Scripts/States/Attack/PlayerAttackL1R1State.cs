

internal class PlayerAttackL1R1State : PlayerAttackState
{
    public PlayerAttackL1R1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.L1R1Attack, 1f);
    }

    public override void OnExit()
    {
        
    }
}
