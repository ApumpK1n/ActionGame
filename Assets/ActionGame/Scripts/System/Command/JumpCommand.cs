

public class JumpCommand : ICommand
{
    public void Execute()
    {
        Game.Instance.GetGameSystem<WorldLogicSystem>().ExecuteCommand(CommandType.Jump);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
