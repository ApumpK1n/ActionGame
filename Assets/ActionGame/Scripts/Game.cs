using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

/// <summary>
/// 这个类目前有多种功能 1:游戏入口功能 2:场景管理 3:场景游戏逻辑 后续需要拆分 
/// </summary>

public class Game : DestroyableSingleton<Game>
{
    GameSystemStack gameSystemStack = new GameSystemStack(3);

    [HideInInspector][NonSerialized] public int dirtySystem = 0;

    [SerializeField] public Player PlayerPrefab;
    [SerializeField] public Transform PlayerReborn;
    [SerializeField] public Weapon StickWeaponPrefab;

    public Player Player;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineFreeLook playerFollowCamera;

    [SerializeField] private Animator debugPlayerCombatAnimator;
    [SerializeField] private Animator debugPlayerMovementAnimator;

    #region Enemy
    [SerializeField] private GoapBehaviour goapBehaviour;
    [SerializeField] private List<Enemy> enemies;
    #endregion

    #region Scene
    [SerializeField, Header("大世界区域")] private List<Transform> Areas;
    #endregion
    private void Awake()
    {
        gameSystemStack.RegisterGameSystem(new LogicSystem());
        gameSystemStack.RegisterGameSystem(new AnimationSystem());
        gameSystemStack.RegisterGameSystem(new CommandInvoker());

        dirtySystem |= (int)SystemType.Logic | (int)SystemType.Animation | (int)SystemType.Command;

        foreach (Enemy enemy in enemies)
        {
            enemy.Setup(goapBehaviour, Areas[0]);
        }
    }


    void Start()
    {
        SetupSystems(dirtySystem);

        Player = Instantiate(PlayerPrefab, PlayerReborn, false);

        playerFollowCamera.Follow = Player.transform;
        playerFollowCamera.LookAt = Player.Neck;

        Player.AddWeapon(StickWeaponPrefab);

        Player.DebugCombatAnimator = debugPlayerCombatAnimator;
        Player.DebugMovementAnimator = debugPlayerMovementAnimator;
    }


    void Update()
    {
        gameSystemStack.Tick(Time.deltaTime * Time.timeScale);

        foreach (Enemy enemy in enemies)
        {
            enemy.Tick(Time.deltaTime * Time.timeScale);
        }
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

    public Transform GetPlayerCamera()
    {
        return playerCamera.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward);

    }
}
