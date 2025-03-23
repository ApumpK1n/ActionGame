using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Units;
using CrashKonijn.Goap.Runtime;
using UniRx;
using UnityEngine;
using UnityHFSM;

public class Player : MonoBehaviour
{
    private AnimationComponent animationComponent;
    public Rigidbody Rigidbody;

    public Transform Head;

    private StateMachine<PlayerStates, Events> fsmRoot;

    public enum MoveMode
    {
        Base = 0,
        Lock = 1,
    }


    public PlayerStates state = PlayerStates.Idle;

    private MoveMode moveMode = MoveMode.Base;
    private PlayerStatesBlackboard blackboard;

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
    #region Unity
    private void Awake()
    {
        animationComponent = GetComponent<AnimationComponent>();
        Rigidbody = GetComponent<Rigidbody>();

        // 后续用事件队列处理并触发Actor行为
        //MessageBroker.Default.Receive<GamePlayJumpLongEvent>().Subscribe(OnJumpLongInput);

        leftFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        footWeights = new AnimatedFloat(animationComponent.Animancer, "LeftFootWeightCurve", "RightFootWeightCurve");
        ApplyAnimatorIK = true;

        blackboard = new PlayerStatesBlackboard();
        blackboard.Player = this;
    }

    private void Start()
    {
        CreateHFSM();
    }

    private void Update()
    {
        fsmRoot.OnLogic();
    }
    #endregion

    private void CreateHFSM()
    {
        fsmRoot = new StateMachine<PlayerStates, Events>();

        var moveFsm = new StateMachine<PlayerStates, MoveStates, Events>();
        var idleFsm = new StateMachine<PlayerStates, IdleStates, Events>();

        fsmRoot.AddState(PlayerStates.Idle, idleFsm);
        fsmRoot.AddState(PlayerStates.Move, moveFsm);
        fsmRoot.AddState(PlayerStates.Jump, new State<PlayerStates, Events>());

        // IDLE
        idleFsm.AddState(IdleStates.BASE, new State<IdleStates, Events>(onEnter: OnEnterBaseIdle));
        idleFsm.SetStartState(IdleStates.BASE);

        // MOVE
        moveFsm.AddState(MoveStates.WALK, new PlayerWalkState(blackboard, false, false));
        moveFsm.AddState(MoveStates.DASH, new PlayerDashState(blackboard, false, false));

        // Transition
        moveFsm.AddTransition(new Transition<MoveStates>(MoveStates.WALK, MoveStates.DASH, condition: WalkToDashCondition));
        moveFsm.AddTransition(new Transition<MoveStates>(MoveStates.DASH, MoveStates.WALK, condition: DashToWalkCondition));

        // IDLE ->MOVE
        fsmRoot.AddTransition(new Transition<PlayerStates>(PlayerStates.Move, PlayerStates.Idle, condition: MoveToIdleCondition));
        // MOVE ->IDLE
        fsmRoot.AddTransition(new Transition<PlayerStates>(PlayerStates.Idle, PlayerStates.Move, condition: IdleToMoveCondition));

        fsmRoot.SetStartState(PlayerStates.Idle);
        fsmRoot.Init();
    }

    private void OnEnterBaseIdle(State<IdleStates, Events> state)
    {
        Debug.Log("OnEnterBaseIdle");
        blackboard.Player.PlayAnimation(AnimationType.Idle);
    }

    #region FsmCondition
    private bool MoveToIdleCondition(Transition<PlayerStates> playerStateTransition)
    {
        return fsmRoot.ActiveState.name == PlayerStates.Move && blackboard.MoveInput.magnitude == 0;
    }

    private bool IdleToMoveCondition(Transition<PlayerStates> playerStateTransition)
    {
        return fsmRoot.ActiveState.name == PlayerStates.Idle && blackboard.MoveInput.magnitude > 0;
    }

    private bool WalkToDashCondition(Transition<MoveStates> moveStateTransition)
    {
        return fsmRoot.ActiveState.name == PlayerStates.Move && blackboard.IsAccelerate;
    }

    private bool DashToWalkCondition(Transition<MoveStates> moveStateTransition)
    {
        return fsmRoot.ActiveState.name == PlayerStates.Move && !blackboard.IsAccelerate;
    }
    #endregion

    private void PlayAnimation(AnimationType animationType)
    {
        animationComponent.Play(animationType);
    }

    public void PlayAnimation(AnimationType animationType, float speed)
    {
        animationComponent.Play(animationType, speed);
    }

    private void OnJumpLongInput(GamePlayJumpLongEvent @event)
    {
        //SwitchState(PlayerState.Jump);
    }

    private void SwitchState(PlayerStates playerState)
    {
        fsmRoot.RequestStateChange(playerState);
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
        }
        transform.forward = targetForward;
        blackboard.MoveInput = dir;
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
    public void SetAccelerate(bool isAccelerate)
    {
        blackboard.IsAccelerate = isAccelerate;
    }

    #endregion
}


public enum PlayerStates
{
    Idle,
    Move,
    Jump,

}

enum MoveStates
{
    WALK, DASH
}

enum IdleStates
{
    BASE,
}

enum Events
{
    ON_DAMAGE, ON_WIN
}

public class PlayerStatesBlackboard
{
    public Player Player { get; set; }
    public bool IsAccelerate { get; set; }
    public Vector2 MoveInput { get; set; }
}
