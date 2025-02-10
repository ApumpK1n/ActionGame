using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AnimationComponent animationComponent;

    private void Awake()
    {
        animationComponent = GetComponent<AnimationComponent>();

        // 后续用事件队列处理并触发Actor行为
        MessageBroker.Default.Receive<GamePlayJumpLongEvent>().Subscribe(OnJumpLongInput);
    }

    private void Start()
    {
        //animationComponent.Play("RightHandAttack");
    }

    private void OnJumpLongInput(GamePlayJumpLongEvent @event)
    {
        animationComponent.Play("JumpBaseLong");
    }
}
