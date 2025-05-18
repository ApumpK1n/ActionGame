

internal class PlayerAttackIdleState : PlayerAttackState
{
    public PlayerAttackIdleState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.IsPlayingWeaponAnimation = true;
        if (blackboard.Player != null)
        {
            blackboard.Player.StopAnimation(PlayerAnimationLayer.HandAttack);
        }
        if (blackboard.CharacterView != null)
        {
            blackboard.CharacterView.StopAnimation(PlayerAnimationLayer.HandAttack);
        }
    }
}
