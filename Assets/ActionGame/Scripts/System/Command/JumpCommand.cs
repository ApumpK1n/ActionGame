

public class JumpCommand : ICommand
{
    public void Execute()
    {
        Game.Instance.GetGameSystem<LogicSystem>().ExecuteCommand(CommandType.Jump);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
