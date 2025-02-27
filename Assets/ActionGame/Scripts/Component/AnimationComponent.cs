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

    public AnimancerComponent Animancer => animancer;
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

    public void Play(AnimationType animationType)
    {
        animancer.Play(animationData.GetAnimationClip(animationType), fadeDuration:0.25f);
    }

    public void Stop()
    {
        animancer.Stop();
    }
}
