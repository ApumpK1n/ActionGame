using UnityEngine;

/// <summary>
/// 游戏入口
/// </summary>
public class GameApp : DestroyableSingleton<GameApp>, IGameApp
{
    // 初始化需要的配置

    [SerializeField] private GameObject PlayerPrefab;   // Player
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private SceneConfig m_StartSceneConfig;

    /// <summary>
    /// 输入模块
    /// </summary>
    [SerializeField] private GamePlayerInput m_StartGamePlayerInput;

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
        // 检查配置参数是否配置
        CheckSerializeVairables();

        // director
        m_Director = new GameDirector();
        m_Director.SetStartSceneConfig(m_StartSceneConfig);
        m_Director.SetPlayerPrefab(PlayerPrefab);
        m_Director.SetEnemyPrefab(EnemyPrefab);
        m_Director.Initialize(m_StartGamePlayerInput);

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

    /// <summary>
    /// 检查一下要输入的参数是否配置
    /// </summary>
    private void CheckSerializeVairables()
    {
        if(m_StartGamePlayerInput == null)
        {
            throw new System.NullReferenceException(nameof(m_StartGamePlayerInput));
        }
    }    
}
