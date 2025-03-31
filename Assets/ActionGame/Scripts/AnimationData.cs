using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: 使用序列化资源替代Mono
public class AnimationData : MonoBehaviour
{
    public List<SingleAnimationData> Animations = new List<SingleAnimationData>();

    public AnimationClip GetAnimationClip(AnimationType type)
    {
        foreach (var animation in Animations)
        {
            if (animation.AnimationType == type)
                return animation.Clip;
        }
        Debug.LogWarning("GetAnimationClip null:" + type);
        return null;
    }
}


[Serializable]
public class SingleAnimationData
{
    public AnimationType AnimationType;
    public AnimationClip Clip;
}

public enum AnimationType
{
    Defence = 2,
    Jump = 3,
    BaseMove = 4,
    Idle = 5,
    L1Attack = 100,
    R1Attack = 101,
    L1R1Attack = 102,
}
