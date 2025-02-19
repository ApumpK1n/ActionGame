

using UniRx;

public class JumpCommand : ICommand
{
    public void Execute()
    {
        MessageBroker.Default.Publish(new GamePlayJumpLongEvent());
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
