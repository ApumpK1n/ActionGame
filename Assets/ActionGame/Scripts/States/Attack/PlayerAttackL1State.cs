
using Animancer;

internal class PlayerAttackL1State : PlayerAttackState
{
    AnimancerState animancerState;

    public PlayerAttackL1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.IsPlayingWeaponAnimation = true;
        animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.L1Attack, 1f);
        animancerState.SetWeight(1f);
        animancerState.Events.NormalizedEndTime = 0.75f;
        animancerState.Events.OnEnd = OnEnd;
    }

    private void OnEnd()
    {
        animancerState.Events.OnEnd = null;
        blackboard.IsPlayingWeaponAnimation = false;
    }

    public override void OnExit()
    {

    }
}
