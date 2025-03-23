
using UnityHFSM;

internal class PlayerWalkState : PlayerMoveState
{
    public PlayerWalkState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }

    public override void Init() { }

    public override void OnEnter()
    {
        blackboard.Player.PlayAnimation(AnimationType.BaseMove, 1f);
    }
    //public override void OnLogic() { }
    //public override void OnExit() { }

    //public override void OnExitRequest() { }
}
