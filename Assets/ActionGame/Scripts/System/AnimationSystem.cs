using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等待扩展。不一定保留
/// </summary>

public class AnimationSystem : IGameSystem
{
    public SystemType TypeEnum
    {
        get
        {
            return SystemType.Animation;
        }
    }

    public void Dispose()
    {
        //throw new System.NotImplementedException();
    }

    public void Setup()
    {
        //throw new System.NotImplementedException();
    }

    public void Start()
    {

    }

    public void Tick(float deltaTime)
    {
        //throw new System.NotImplementedException();
    }
}
