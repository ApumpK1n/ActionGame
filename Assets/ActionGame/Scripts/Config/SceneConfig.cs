

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景配置
/// </summary>
[CreateAssetMenu]
public class SceneConfig : ScriptableObject
{
    [Header("场景名")] public string Name;
    [Header("场景描述")] public string Description;
    public LoadSceneMode LoadSceneMode;
}
