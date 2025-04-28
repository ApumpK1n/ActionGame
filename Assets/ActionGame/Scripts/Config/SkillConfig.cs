

using UnityEngine;

[CreateAssetMenu]
public class SkillConfig : ScriptableObject
{
    [Header("技能Id")] public int Id;
    [Header("描述")] public string Description;
    [Header("冷却时间s")] public int Cooldown;
    [Header("施法前摇")] public float CastPoint;
}
