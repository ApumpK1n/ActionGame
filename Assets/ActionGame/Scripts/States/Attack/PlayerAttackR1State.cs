
using Animancer;

internal class PlayerAttackR1State : PlayerAttackState
{
    AnimancerState animancerState;
    public PlayerAttackR1State(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
: base(blackboard, needsExitTime, isGhostState) { }
    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.IsPlayingWeaponAnimation = true;
        //animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.R1Attack, 1f);
        if (blackboard.Player != null)
        {
            animancerState = blackboard.Player.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.R1Attack, 1f);
        }
        if (blackboard.CharacterView != null)
        {
            animancerState = blackboard.CharacterView.PlayAnimation(PlayerAnimationLayer.HandAttack, AnimationType.R1Attack, 1f);
        }
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

    }
}
