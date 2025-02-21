using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicSystem : IGameSystem
{
    public SystemType TypeEnum
    {
        get
        {
            return SystemType.Logic;
        }
    }

    public void Dispose()
    {

    }

    public void Setup()
    {
        
    }

    public void Tick(float deltaTime)
    {

    }

    public void PlayerMove(Vector2 dir)
    {
        Game.Instance.Player.Move(dir);
    }
}
