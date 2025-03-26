

public class AttackCommand : ICommand
{
    public bool LeftClick = false;
    public bool RightClick = false;
    public void Execute()
    {
        if (LeftClick && RightClick)
        {
            Game.Instance.GetGameSystem<LogicSystem>().ExecuteCommand(CommandType.BothClick);
        }
        else if (LeftClick)
        {
            Game.Instance.GetGameSystem<LogicSystem>().ExecuteCommand(CommandType.LeftAttack);
        }
        else if (RightClick)
        {
            Game.Instance.GetGameSystem<LogicSystem>().ExecuteCommand(CommandType.RightAttack);
        }
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
