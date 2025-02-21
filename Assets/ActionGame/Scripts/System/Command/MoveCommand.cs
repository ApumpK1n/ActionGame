using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector2 MoveDir;

    public void Execute()
    {
        Game.Instance.GetGameSystem<LogicSystem>().PlayerMove(MoveDir);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
