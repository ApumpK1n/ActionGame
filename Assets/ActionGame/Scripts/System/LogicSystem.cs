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

    // TODO:抽象出属性
    public void PlayerAccelerate(bool isAccelerate)
    {
        Game.Instance.Player.SetAccelerate(isAccelerate);
    }

    public void PlayerJump()
    {
        //Game.Instance.Player.TryEnterJumpState();
    }

    public void LeftClick()
    {

    }

    // TODO: 统一执行Command接口
    public void ExecuteCommand(CommandType commandType)
    {
        Game.Instance.Player.ExecuteCommand(commandType);
    }
}
