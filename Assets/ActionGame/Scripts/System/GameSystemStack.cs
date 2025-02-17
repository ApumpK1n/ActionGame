using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemStack
{
    private List<IGameSystem> stack;

    public GameSystemStack(int capacity)
    {
        stack = new List<IGameSystem>(capacity);
    }

    public void Setup(int dirtyFlags)
    {
        foreach (var system in stack)
            if ((dirtyFlags & (int)system.TypeEnum) != 0)
                system.Setup();
    }

    public void Tick(float deltaTime)
    {
        foreach (var system in stack)
            system.Tick(deltaTime);
    }

    public bool RegisterGameSystem(IGameSystem gameSystem)
    {
        if (gameSystem != null)
        {
            stack.Add(gameSystem);
            return true;
        }
        return false;
    }

    public bool UnregisterRenderSystem(IGameSystem gameSystem)
    {
        if (gameSystem != null)
        {
            return stack.Remove(gameSystem);
        }
        return false;
    }

    public T GetGameSystem<T>() where T : IGameSystem
    {
        foreach(var system in stack)
        {
            if (system.GetType() == typeof(T))
            {
                return (T)system;
            }
        }
        return default(T);
    }
}
