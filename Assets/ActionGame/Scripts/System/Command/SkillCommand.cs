
using UnityEngine;

public class SkillCommand : ICommand
{
    public int SkillSlot;

    public void Execute()
    {
        Game.Instance.GetGameSystem<WorldLogicSystem>().ExecuteSkillCommand(SkillSlot);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
