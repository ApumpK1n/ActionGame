using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 考虑使用序列化资源替代类
public class AnimationData : MonoBehaviour
{
    public List<SingleAnimationData> Animations = new List<SingleAnimationData>();

    public AnimationClip GetAnimationClip(string name)
    {
        foreach (var animation in Animations)
        {
            if (animation.Name == name)
                return animation.Clip;
        }
        return null;
    }
}


[Serializable]
public class SingleAnimationData
{
    public string Name;
    public AnimationType AnimationType;
    public AnimationClip Clip;
}

public enum AnimationType
{
    Attack,
    Defence,
    Jump,
}
