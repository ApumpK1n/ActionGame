using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        throw new System.NotImplementedException();
    }

    public void Setup()
    {
        throw new System.NotImplementedException();
    }

    public void Tick(float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
