
public interface ICommand
{
    public void Execute();
    public void Undo();
}


public enum CommandType
{
    Accelerate  = 1,
    Jump        = 2,
    Move        = 3,
    LeftAttack  = 4,
    RightAttack = 5,
    BothClick   = 6,
    Skill       = 7,
}
