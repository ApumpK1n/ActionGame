using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public AnimationClip Clip;
}
