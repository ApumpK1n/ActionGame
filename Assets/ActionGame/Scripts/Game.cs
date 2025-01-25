using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    GameSystemStack gameSystemStack = new GameSystemStack(3);

    [HideInInspector][NonSerialized] public int dirtySystem = 0;

    private void Awake()
    {
        gameSystemStack.RegisterGameSystem(new LogicSystem());
        gameSystemStack.RegisterGameSystem(new AnimationSystem());

        dirtySystem |= (int)SystemType.Logic | (int)SystemType.Animation;
    }
    void Start()
    {
        SetupSystems(dirtySystem);
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
}
