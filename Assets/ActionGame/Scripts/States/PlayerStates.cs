using UnityHFSM;

internal class PlayerMoveState : StateBase<MoveStates>
{
    protected PlayerStatesBlackboard blackboard;

    public PlayerMoveState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
      : base(needsExitTime, isGhostState)
    {
        this.blackboard = blackboard;
    }

    public override void Init() { }

    public override void OnEnter()
    {
       // blackboard.Player
    }
    public override void OnLogic() { }
    public override void OnExit() { }

    public override void OnExitRequest() { }
}
