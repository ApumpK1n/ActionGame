

public class JumpCommand : ICommand
{
    public void Execute()
    {
        Game.Instance.GetGameSystem<LogicSystem>().PlayerJump();
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
