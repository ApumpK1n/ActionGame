

internal class PlayerAttackIdleState : PlayerAttackState
{
    public PlayerAttackIdleState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.IsPlayingWeaponAnimation = true;
        blackboard.Player.StopAnimation(PlayerAnimationLayer.HandAttack);
    }
}
