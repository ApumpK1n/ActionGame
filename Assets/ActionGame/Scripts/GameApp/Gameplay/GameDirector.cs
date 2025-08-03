using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 狭义上游戏玩法的总入口
/// </summary>
public class GameDirector
{
    /// <summary>
    /// 场景管理
    /// </summary>
    private World m_World;

    /// <summary>
    /// 玩家控制角色的控制器
    /// </summary>
    private PlayerController m_PlayerController;

    /// <summary>
    /// subsystems
    /// </summary>
    private SubsystemCollection<IGameDirectorSubsystem> m_Subsystems;

    private SceneConfig m_StartSceneConfig;
    private GameObject m_PlayerPrefab;  // 玩家控制角色的预制体

    private bool m_Initialized;

    public GameDirector()
    { }

    public void Initialize(GamePlayerInput playerInput, Camera camera)
    {
        // world

        m_World = World.CreateWorld();
        m_World.SetGameDirector(this);

        // subsystems
        m_Subsystems = new SubsystemCollection<IGameDirectorSubsystem>();
        m_Subsystems.Initialize();

        // PlayerController
        m_PlayerController = m_World.SpawnPlayerController();
        // input
        m_PlayerController.InitializePlayerInput(playerInput);
        // camera
        m_PlayerController.SpawnPlayerCameraManager(camera);
        // 实例化之后的初始化
        m_PlayerController.Initialize();

        m_Initialized = true;
    }

    public void DeInitialize()
    {
        m_Subsystems.Deinitialize();
    }

    public void SetStartSceneConfig(SceneConfig startSceneConfig)
    {
        if(startSceneConfig == null)
        {
            throw new ArgumentNullException(nameof(startSceneConfig));
        }

        m_StartSceneConfig = startSceneConfig;
    }

    public void SetPlayerPrefab(GameObject playerPrefab)
    {
        if (playerPrefab == null)
        {
            throw new ArgumentNullException(nameof(playerPrefab));
        }

        m_PlayerPrefab = playerPrefab;
    }

    public bool StartGame()
    {
        if(!m_Initialized)
        {
            return false;
        }

        // 没有启动配置，先返回失败
        if(m_StartSceneConfig == null || m_PlayerPrefab == null)
        {
            return false;
        }

        bool canLoadLevel = m_World.LoadLevel(m_StartSceneConfig.Name, m_StartSceneConfig.LoadSceneMode, OnStartLevelLoaded);
        if (!canLoadLevel)
        {
            return false;
        }

        return true;
    }

    public void Tick(float deltaTime)
    {
        if(!m_Initialized)
        {
            return;
        }
    }

    public TSubSystem GetSubsystem<TSubSystem>() where TSubSystem : IGameDirectorSubsystem, new()
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
            subsystem.SetGameDirector(this);
            m_Subsystems.RegisterSubsystem(subsystem);
        }

        return subsystem;
    }

    public IGameDirectorSubsystem GetSubsystemBase()
    {
        if (m_Subsystems == null)
        {
            return null;
        }

        return m_Subsystems.GetSubsystem<IGameDirectorSubsystem>();
    }

    // ********************************************************************************************************

    /// <summary>
    /// 启动场景加载成功之后的处理
    /// </summary>
    private void OnStartLevelLoaded(string levelName, bool success)
    {
        if(string.IsNullOrEmpty(levelName) || m_StartSceneConfig.Name != levelName)
        {
            // 加载的不对，不处理
            return;
        }

        if (!success)
        {
            return;
        }

        // 加载成功之后的处理，这里主要是创角，然后加入场景中显示

        GameObject playerObject = GameObject.Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        CharacterView player = playerObject.GetComponent<CharacterView>();
        //player.Setup(this);

        Character character = m_PlayerController.SpawnCharacter();
        character.BindCharacterView(player);

        // 获取表现层角色节点
        GameObject levelNode = GameObject.Find("LevelNode");
        if (levelNode != null)
        {
            SceneViewLogic sceneViewLogic = levelNode.GetComponent<SceneViewLogic>();
            if (sceneViewLogic != null)
            {
                sceneViewLogic.AddPlayerTo(player);

                player.OnSetup();
            }
        }
    }

    // ********************************************************************************************************
}
