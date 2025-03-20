using UnityEngine;

public class AccelerateCommand : ICommand
{
    public bool Value;
    public void Execute()
    {
        Game.Instance.GetGameSystem<LogicSystem>().PlayerAccelerate(Value);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
