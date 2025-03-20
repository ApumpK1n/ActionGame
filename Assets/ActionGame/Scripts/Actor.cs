using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Units;
using UniRx;
using UnityEngine;
public class Actor : MonoBehaviour
{
    private AnimationComponent animationComponent;
    public Rigidbody Rigidbody;

    public Transform Head;
    public enum MoveMode
    {
        Base = 0,
        Lock = 1,
    }

    // TODO:先用有限状态机 后续需要抽象分层状态机 并剥离代码
    public enum PlayerState
    {
        Idle,
        Move,
        Jump,

    }

    public PlayerState state = PlayerState.Idle;

    private MoveMode moveMode = MoveMode.Base;

    public Vector3 targetForward;

    private Transform leftFoot;
    private Transform rightFoot;
    private AnimatedFloat footWeights;
    [SerializeField, Meters] private float _RaycastOriginY = 0.5f;
    [SerializeField, Meters] private float _RaycastEndY = -0.2f;

    public bool ApplyAnimatorIK
    {
        get => animationComponent.Animancer.Layers[0].ApplyAnimatorIK;
        set => animationComponent.Animancer.Layers[0].ApplyAnimatorIK = value;
    }

    private void Awake()
    {
        animationComponent = GetComponent<AnimationComponent>();
        Rigidbody = GetComponent<Rigidbody>();

        // 后续用事件队列处理并触发Actor行为
        MessageBroker.Default.Receive<GamePlayJumpLongEvent>().Subscribe(OnJumpLongInput);

        leftFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        footWeights = new AnimatedFloat(animationComponent.Animancer, "LeftFootWeightCurve", "RightFootWeightCurve");
        ApplyAnimatorIK = true;
    }

    private void Start()
    {
        SwitchState(PlayerState.Idle);
    }

    private void SwitchState(PlayerState state)
    {
        this.state = state;
        switch (state)
        {
            case PlayerState.Idle:
                animationComponent.Play(AnimationType.Idle);
                break;
            case PlayerState.Move:
                animationComponent.Play(AnimationType.BaseMove, GetSpeed());
                break;
            case PlayerState.Jump:
                animationComponent.Play(AnimationType.Jump);
                break;
        }
    }

    private void OnJumpLongInput(GamePlayJumpLongEvent @event)
    {
        SwitchState(PlayerState.Jump);
    }
    #region Move
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
        bool isMove = false;
        if (dir.x != 0 && dir.y != 0)
        {
            if (dir.y > 0 && dir.x < 0) // leftForward
            {
                //anim.SetTrigger("move_up_left");
                //targetEulerAngles = new Vector3(0, -45, 0);
                targetForward = GetTargetForward(-45);
                isMove = true;
            }

            if (dir.y > 0 && dir.x > 0) // rightForward
            {
                //anim.SetTrigger("move_up_right");
                //targetEulerAngles = new Vector3(0, 45, 0);
                targetForward = GetTargetForward(45);
                isMove = true;
            }

            if (dir.y < 0 && dir.x < 0) // backleft
            {
                //anim.SetTrigger("move_down_left");
                //targetEulerAngles = new Vector3(0, -135, 0);
                targetForward = GetTargetForward(-135);
                isMove = true;
            }

            if (dir.y < 0 && dir.x > 0) // backright
            {
                //anim.SetTrigger("move_down_right");
                //targetEulerAngles = new Vector3(0, 135, 0);
                targetForward = GetTargetForward(135);
                isMove = true;
            }
        }

        else
        {

            //left/right/up/down
            if (dir.x < 0)
            {
                //targetForward = Vector3.left;
                targetForward = GetTargetForward(-90);
                isMove = true;
                //anim.SetTrigger("move_left");
            }

            if (dir.x > 0)
            {
                targetForward = GetTargetForward(90);
                isMove = true;
                //anim.SetTrigger("move_right");
            }


            if (dir.y > 0)
            {
                targetForward = GetTargetForward(0);
                isMove = true;
                //anim.SetTrigger("move_up");
            }


            if (dir.y < 0)
            {
                targetForward = GetTargetForward(180);
                isMove = true;
                //anim.SetTrigger("move_down");
            }

            if (dir.x == 0 && dir.y == 0)
            {
                animationComponent.Play(AnimationType.Idle);
            }
        }

        transform.forward = targetForward;
        if (isMove)
        {
            SwitchState(PlayerState.Move);
        }
    }

    /// <summary>
    /// get current Camera forward * rotate angle
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 GetTargetForward(float angle)
    {
        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 rotation = quaternion* Game.Instance.GetPlayerCamera().forward;
        return new Vector3(rotation.x, 0, rotation.z);
    }

    private void OnAnimatorMove()
    {
        //animationComponent.Animancer.Animator.ApplyBuiltinRootMotion();
        // Rigidbody
        Rigidbody.MovePosition(Rigidbody.position + animationComponent.Animancer.Animator.deltaPosition);
        //Rigidbody.MoveRotation(Rigidbody.rotation * animationComponent.Animancer.Animator.deltaRotation);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + targetForward * 10);
    }

    #region IK
    // Note that due to limitations in the Playables API, Unity will always call this method with layerIndex = 0.
    private void OnAnimatorIK(int layerIndex)
    {
        // _FootWeights[0] is the first property we specified in Awake: "LeftFootIK".
        // _FootWeights[1] is the second property we specified in Awake: "RightFootIK".
        UpdateFootIK(leftFoot, AvatarIKGoal.LeftFoot, footWeights[0], animationComponent.Animancer.Animator.leftFeetBottomHeight);
        UpdateFootIK(rightFoot, AvatarIKGoal.RightFoot, footWeights[1], animationComponent.Animancer.Animator.rightFeetBottomHeight);
    }

    /************************************************************************************************************************/

    private void UpdateFootIK(Transform footTransform, AvatarIKGoal goal, float weight, float footBottomHeight)
    {
        var animator = animationComponent.Animancer.Animator;

        if (weight == 0) return;

        // Get the local up direction of the foot.
        var rotation = animator.GetIKRotation(goal);
        var localUp = rotation * Vector3.up;

        var position = footTransform.position;
        position += localUp * _RaycastOriginY;

        var distance = _RaycastOriginY - _RaycastEndY;

        LayerMask mask = 1 << LayerMask.NameToLayer("Occluder");
        if (Physics.Raycast(position, -localUp, out var hit, distance, mask))
        {
            animator.SetIKPositionWeight(goal, weight);
            // Use the hit point as the desired position.
            position = hit.point;
            position += localUp * footBottomHeight;
            animator.SetIKPosition(goal, position);

            // Use the hit normal to calculate the desired rotation.
            var rotAxis = Vector3.Cross(localUp, hit.normal);
            var angle = Vector3.Angle(localUp, hit.normal);
            rotation = Quaternion.AngleAxis(angle, rotAxis) * rotation;

            animator.SetIKRotation(goal, rotation);

        }
        else// Otherwise simply stretch the leg out to the end of the ray.
        {
            //position += localUp * (footBottomHeight - distance);
            //animator.SetIKPosition(goal, position);
            animator.SetIKPositionWeight(goal, 0);
        }
    }
    #endregion

    #region Accelerate

    private float AccelerateSpeed = 2f;
    private bool isAccelerate = false;
    public void SetAccelerate(bool isAccelerate)
    {
        this.isAccelerate = isAccelerate;
    }

    private float GetSpeed()
    {
        if (isAccelerate) return AccelerateSpeed;
        return 1f;
    }
    #endregion
}
