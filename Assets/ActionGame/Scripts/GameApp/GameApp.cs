using UnityEngine;

/// <summary>
/// 游戏入口
/// </summary>
public class GameApp : DestroyableSingleton<GameApp>, IGameApp
{
    // 初始化需要的配置

    [SerializeField] private GameObject PlayerPrefab;   // Player
    //[SerializeField] private WorldConfig m_WorldConfig;
    [SerializeField] private SceneConfig m_StartSceneConfig;

    private GameDirector m_Director;

    private SubsystemCollection<IGameAppSubsystem> m_Subsystems;

    private bool m_Initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Tick(Time.deltaTime);
    }

    void OnDestroy()
    {
        OnShutDown();
    }

    #region IGameApp

    public void Initialize()
    {
        // director
        m_Director = new GameDirector();
        m_Director.SetStartSceneConfig(m_StartSceneConfig);
        m_Director.SetPlayerPrefab(PlayerPrefab);
        m_Director.Initialize();

        // subsytem
        m_Subsystems = new SubsystemCollection<IGameAppSubsystem>();
        m_Subsystems.Initialize();


        m_Initialized = true;
    }

    public TSubSystem GetSubsystem<TSubSystem>() where TSubSystem : IGameAppSubsystem, new()
    {
        if (m_Subsystems == null)
        {
            return default;
        }

        TSubSystem subsystem = m_Subsystems.GetSubsystem<TSubSystem>();
        if (subsystem == null && m_Initialized)
        {
            // 这样去支持懒加载，不知道是否有什么副作用
            subsystem = new TSubSystem();
            subsystem.Setup();
            m_Subsystems.RegisterSubsystem(subsystem);
        }

        return subsystem;
    }

    public IGameAppSubsystem GetSubsystemBase()
    {
        if (m_Subsystems == null)
        {
            return null;
        }

        return m_Subsystems.GetSubsystem<IGameAppSubsystem>();
    }

    /// <summary>
    /// 启动场景切换到游戏场景
    /// </summary>
    public void StartGame()
    {
        //SceneManager.LoadSceneAsync("ActionDemoScene ");
        if(!m_Initialized)
        {
            return;
        }

        m_Director?.StartGame();
    }

    public void OnFoucs(bool onFoucs)
    {
        
    }

    public void OnShutDown()
    {
        
    }

    public void Tick(float dt)
    {
        if (!m_Initialized)
        {
            return;
        }

        m_Director?.Tick(dt);

        if(m_Subsystems != null)
        {
            m_Subsystems.Tick(dt);
        }
    }

    #endregion
}
