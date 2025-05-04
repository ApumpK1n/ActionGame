using System.Collections.Generic;
using System;
using UnityEngine;

public class SubsystemCollection<TSubsystem> where TSubsystem : IGameSystem
{
    private Dictionary<Type, IGameSystem> m_SubsystemMap = new Dictionary<Type, IGameSystem>();

    public SubsystemCollection()
    {
        m_SubsystemMap.Clear();
    }

    public void Initialize() { }

    public void Deinitialize() { }

    public void Tick(float deltaTime)
    {
        foreach (var mapItem in m_SubsystemMap.Values)
        {
            mapItem.Tick(deltaTime);
        }
    }

    public void RegisterSubsystem(TSubsystem subsystem)
    {
        Type type = subsystem.GetType();
        m_SubsystemMap.TryAdd(type, subsystem);
    }

    public void UnregisterSubsystem(TSubsystem subsystem)
    {
        Type type = subsystem.GetType();
        m_SubsystemMap.Remove(type);
    }

    public TConcreteSubsystem GetSubsystem<TConcreteSubsystem>() where TConcreteSubsystem : IGameSystem
    {
        Type subsystemType = typeof(TConcreteSubsystem);
        if (m_SubsystemMap.TryGetValue(subsystemType, out IGameSystem value))
        {
            return (TConcreteSubsystem)value;
        }

        return default;
    }
}
