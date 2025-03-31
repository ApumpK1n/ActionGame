using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using System;

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

    public void Play(int layer, AnimationClip animationClip)
    {
        animancer.Play(animationClip);
    }

    public AnimancerState Play(int layer, AnimationType animationType, float speed=1.0f, FadeMode fadeMode=default, Action<AnimancerState> onEnd=null)
    {
        AnimationClip clip = animationData.GetAnimationClip(animationType);
        if (clip == null) return null;
        AnimancerState animancerState;

        if (animancer.States.Current != null && animancer.States.Current.IsValid()
            && animancer.States.Current.IsActive && animancer.States.Current.Clip.name == clip.name)
        {
            animancerState = animancer.States.Current;
        }
        else
        {
            animancerState = animancer.Layers[layer].Play(clip, fadeDuration: 0.25f, mode:fadeMode);
        }
        animancerState.Speed = speed;
        animancerState.LayerIndex = layer;
        if (onEnd != null)
        {
            animancerState.Events.OnEnd = ()=> onEnd?.Invoke(animancerState);
        }
        return animancerState;
    }

    public void Stop(int layer)
    {
        animancer.Layers[layer].Stop();
    }

    public void SetCurrentAnimationSpeed(float speed)
    {
        var animancerState = animancer.States.Current;
        if (animancerState != null)
        {
            animancerState.Speed = speed;
        }

    } 

    public void Stop()
    {
        animancer.Stop();
    }
}
