

using UnityEngine;

[CreateAssetMenu]
public class CharacterConfig : ScriptableObject
{
    [Header("移动转向速度")] public float MoveTurnSpeed = 0.5f;
    [Header("基础移速")] public float BaseMoveSpeed = 1f;
}
