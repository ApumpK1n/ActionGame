using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AnimationComponent animationComponent;

    private void Awake()
    {
        animationComponent = GetComponent<AnimationComponent>();
    }

    private void Start()
    {
        animationComponent.Play("RightHandAttack");
    }
}
