using UnityHFSM;
using UnityEngine;

public class PlayerInSkyState : StateBase<InSkyStates>
{
    protected PlayerStatesBlackboard blackboard;

    public PlayerInSkyState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
      : base(needsExitTime, isGhostState)
    {
        this.blackboard = blackboard;
    }

    public override void Init() { }

    public override void OnEnter()
    {

    }
    public override void OnLogic()
    {

    }
    public override void OnExit() { }

    public override void OnExitRequest() { }
}
