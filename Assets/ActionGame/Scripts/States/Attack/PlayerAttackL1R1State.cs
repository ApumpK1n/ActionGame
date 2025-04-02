

using Animancer;

internal class PlayerAttackL1R1State : PlayerAttackState
{
    AnimancerState animancerState;

    public PlayerAttackL1R1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.IsPlayingWeaponAnimation = true;
        animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.L1R1Attack, 1f);
        animancerState.SetWeight(1f);
        animancerState.Events.NormalizedEndTime = 0.6f;
        animancerState.Events.OnEnd = OnEnd;
    }

    private void OnEnd()
    {
        animancerState.Events.OnEnd = null;
        blackboard.IsPlayingWeaponAnimation = false;
    }

    public override void OnExit()
    {
        blackboard.IsPlayingWeaponAnimation = false;
    }
}
