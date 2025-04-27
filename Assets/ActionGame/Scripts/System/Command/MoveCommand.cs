using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector2 MoveDir;

    public void Execute()
    {
        Game.Instance.GetGameSystem<WorldLogicSystem>().PlayerMove(MoveDir);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
