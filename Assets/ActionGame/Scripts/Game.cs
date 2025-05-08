using System;
using UnityEngine;

/// <summary>
/// 游戏入口功能: 此类为单例不销毁 管理所有游戏系统
/// </summary>

public class Game : DestroyableSingleton<Game>
{
    GameSystemStack gameSystemStack = new GameSystemStack(3);

    [HideInInspector][NonSerialized] public int dirtySystem = 0;

    [SerializeField] private WorldConfig worldConfig;

    public GameSystemStack GameSystemStack => gameSystemStack;

    #region Unity
    private void Awake()
    {
        SetupSystems();

        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        gameSystemStack.Start();
        gameSystemStack.GetGameSystem<WorldLogicSystem>().LoadWorld(worldConfig);
    }


    void Update()
    {
        gameSystemStack.Tick(Time.deltaTime * Time.timeScale);
    }

    private void OnDestroy()
    {
        
    }
    #endregion
    private void SetupSystems()
    {
        gameSystemStack.RegisterGameSystem(new WorldLogicSystem());
        gameSystemStack.RegisterGameSystem(new AnimationSystem());
        gameSystemStack.RegisterGameSystem(new CommandInvoker());

        dirtySystem |= (int)SystemType.WorldLogic | (int)SystemType.Animation | (int)SystemType.Command;
        SetupSystems(dirtySystem);
    }

    public void SetupSystems(int dirtyFlags)
    {
        dirtySystem = dirtyFlags;
        if (dirtySystem != 0)
        {
            gameSystemStack.Setup(dirtySystem);
            dirtySystem = 0;
        }

    }

    public T GetGameSystem<T>() where T : IGameSystem
    {
        return gameSystemStack.GetGameSystem<T>();
    }
}
