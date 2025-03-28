
using UnityHFSM;

internal class PlayerJumpState : PlayerInSkyState
{
    public PlayerJumpState(PlayerStatesBlackboard blackboard, bool needsExitTime, bool isGhostState)
        : base(blackboard, needsExitTime, isGhostState) { }

    public override void Init() { }

    public override void OnEnter()
    {

    }
}
