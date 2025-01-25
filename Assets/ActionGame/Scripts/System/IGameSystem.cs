using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameSystem
{
    void Setup();

    void Tick(float deltaTime);

    void Dispose();

    // Type GetSystemType();
    SystemType TypeEnum { get; }
}
