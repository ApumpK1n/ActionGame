using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(AnimationData))]
public class AnimationComponent : MonoBehaviour
{

    private AnimancerComponent animancer;
    private AnimationData animationData;

    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
        animationData = GetComponent<AnimationData>();
    }

    public void Play(AnimationClip animationClip)
    {
        animancer.Play(animationClip);
    }

    public void Play(string animationName)
    {
         animancer.Play(animationData.GetAnimationClip(animationName));
    }
}
