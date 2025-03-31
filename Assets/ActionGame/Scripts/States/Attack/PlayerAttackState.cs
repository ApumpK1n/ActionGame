using UnityHFSM;

internal class PlayerAttackState : StateBase<WeaponStates>
{
    protected PlayerStatesBlackboard blackboard;

    public PlayerAttackState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
      : base(needsExitTime, isGhostState)
    {
        this.blackboard = blackboard;
    }

    public override void Init() { }

    public override void OnEnter()
    {
        
    }
    public override void OnLogic() { }
    public override void OnExit() { }

    public override void OnExitRequest() { }
}
