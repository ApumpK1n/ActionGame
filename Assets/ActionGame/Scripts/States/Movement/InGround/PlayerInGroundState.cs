using UnityHFSM;

public class PlayerInGroundState : StateBase<InGroundStates>
{
    protected PlayerStatesBlackboard blackboard;

    public PlayerInGroundState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
      : base(needsExitTime, isGhostState)
    {
        this.blackboard = blackboard;
    }

    public override void Init() { }

    public override void OnEnter(){ }
    public override void OnLogic() { }
    public override void OnExit() { }

    public override void OnExitRequest() { }
}
