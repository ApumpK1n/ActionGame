
using UnityHFSM;

internal class PlayerWalkState : PlayerInGroundState
{
    public PlayerWalkState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }

    public override void Init() { }

    public override void OnEnter()
    {
        if (blackboard.Player != null)
        {
            blackboard.Player.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.BaseMove, blackboard.CharacterConfig.WalkMoveSpeed);
        }

        if (blackboard.CharacterView != null)
        {
            blackboard.CharacterView.PlayAnimation(PlayerAnimationLayer.Base, AnimationType.BaseMove, blackboard.CharacterConfig.WalkMoveSpeed);
        }
    }
    //public override void OnLogic() { }
    //public override void OnExit() { }

    //public override void OnExitRequest() { }
}
