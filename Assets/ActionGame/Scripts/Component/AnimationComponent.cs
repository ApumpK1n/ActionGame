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

    public void Play(AnimationType animationType, float speed=1.0f)
    {
        AnimationClip clip = animationData.GetAnimationClip(animationType);
        if (clip == null) return;
        AnimancerState animancerState;

        if (animancer.IsPlaying(clip))
        {
            animancerState = animancer.States.Current;
        }
        else
        {
            animancerState = animancer.Play(clip, fadeDuration: 0.25f);
        }
        animancerState.Speed = speed;
        Debug.Log("animancerState:" + animancerState + "," +speed);
    }

    public void Stop()
    {
        animancer.Stop();
    }
}
