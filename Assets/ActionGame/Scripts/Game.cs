using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Game : DestroyableSingleton<Game>
{
    GameSystemStack gameSystemStack = new GameSystemStack(3);

    [HideInInspector][NonSerialized] public int dirtySystem = 0;

    [SerializeField] public Actor PlayerPrefab;
    [SerializeField] public Transform PlayerReborn;

    public Actor Player;

    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineFreeLook playerFollowCamera;
    private void Awake()
    {
        gameSystemStack.RegisterGameSystem(new LogicSystem());
        gameSystemStack.RegisterGameSystem(new AnimationSystem());
        gameSystemStack.RegisterGameSystem(new CommandInvoker());

        dirtySystem |= (int)SystemType.Logic | (int)SystemType.Animation | (int)SystemType.Command;
    }
    void Start()
    {
        SetupSystems(dirtySystem);

        Player = Instantiate(PlayerPrefab, PlayerReborn, false);

        playerFollowCamera.Follow = Player.transform;
        playerFollowCamera.LookAt = Player.Head;
    }


    void Update()
    {
        gameSystemStack.Tick(Time.deltaTime * Time.timeScale);
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
