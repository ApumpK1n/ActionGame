public class GameInputCommandInvoker : IGameDirectorSubsystem
{
    private GameDirector m_GameDirector;

    public GameInputCommandInvoker()
    {
    }

    public SystemType TypeEnum => SystemType.Command;

    public void SetGameDirector(GameDirector gameDirector)
    {
        if (m_GameDirector == null)
        {
            throw new System.ArgumentNullException(nameof(m_GameDirector));
        }

        m_GameDirector = gameDirector;
    }

    public void Start()
    {

    }

    public void Dispose()
    {
        m_GameDirector = null;
    }

    public GameDirector GetGameDirector()
    {
        return m_GameDirector;
    }

    public void Setup()
    {
        
    }

    public void Tick(float deltaTime)
    {
        
    }


    
}
