using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;

public class Actor : MonoBehaviour
{
    private AnimationComponent animationComponent;

    public enum MoveMode
    {
        Base = 0,
        Lock = 1,
    }

    public enum PlayerState
    {
        Idle,
        
    }

    private MoveMode moveMode = MoveMode.Base;
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
        animationComponent.Play(AnimationType.Jump);
    }

    public void Move(Vector2 dir)
    {
        switch (moveMode)
        {
            case MoveMode.Base:
                BaseMove(dir);
                break;
        }
    }

    private void BaseMove(Vector2 dir)
    {
        if (dir.x != 0 && dir.y != 0)
        {
            if (dir.y > 0 && dir.x < 0)
            {
                //anim.SetTrigger("move_up_left");
                transform.eulerAngles = new Vector3(0, -45, 0);
                animationComponent.Play(AnimationType.BaseMove);
            }

            if (dir.y > 0 && dir.x > 0)
            {
                //anim.SetTrigger("move_up_right");
                transform.eulerAngles = new Vector3(0, 45, 0);
                animationComponent.Play(AnimationType.BaseMove);
            }

            if (dir.y < 0 && dir.x < 0)
            {
                //anim.SetTrigger("move_down_left");
                transform.eulerAngles = new Vector3(0, -135, 0);
                animationComponent.Play(AnimationType.BaseMove);
            }

            if (dir.y < 0 && dir.x > 0)
            {
                //anim.SetTrigger("move_down_right");
                transform.eulerAngles = new Vector3(0, 135, 0);
                animationComponent.Play(AnimationType.BaseMove);
            }
        }

        else
        {

            //left/right/up/down
            if (dir.x < 0)
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
                animationComponent.Play(AnimationType.BaseMove);
                //anim.SetTrigger("move_left");
            }

            if (dir.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                animationComponent.Play(AnimationType.BaseMove);
                //anim.SetTrigger("move_right");
            }


            if (dir.y > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                animationComponent.Play(AnimationType.BaseMove);
                //anim.SetTrigger("move_up");
            }


            if (dir.y < 0)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                animationComponent.Play(AnimationType.BaseMove);
                //anim.SetTrigger("move_down");
            }

            if (dir.x == 0 && dir.y == 0)
            {
                animationComponent.Play(AnimationType.Idle);
            }
        }
    }
    private void OnAnimatorMove()
    {
        animationComponent.Animancer.Animator.ApplyBuiltinRootMotion();
    }
}
