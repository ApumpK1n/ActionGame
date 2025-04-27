

using Cinemachine;
using CrashKonijn.Goap.Runtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 世界配置 例:局内或者局外
/// </summary>
[CreateAssetMenu]
public class WorldConfig : ScriptableObject
{
    [Header("场景配置")]public List<SceneConfig> SceneConfigs;
}
