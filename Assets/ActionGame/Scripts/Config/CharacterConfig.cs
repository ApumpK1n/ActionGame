

using UnityEngine;

[CreateAssetMenu]
public class CharacterConfig : ScriptableObject
{
    [Header("移动转向速度")] public float MoveTurnSpeed = 0.5f;
    [Header("行走移速")] public float WalkMoveSpeed = 2f;
    [Header("小跑移速")] public float DashMoveSpeed = 3f;

}
