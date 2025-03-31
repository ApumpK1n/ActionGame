using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Units;
using UniRx;
using UnityEngine;
using UnityHFSM;
using System;

/*
  HFSM
- Root (根状态) 移动和战斗并行
  - Movement (移动层)
    - 地上移动 （子状态互斥）
        - Idle (待机)
        - Walk (走）
        - Dash (跑)
    - 空中移动 （子状态互斥）
        - 跳
        - 坠落
  - Combat (战斗层)
    - NormalAttack (普通攻击)
    - SkillAttack (技能攻击)
  - 其他全局状态（如受伤、死亡）
 
 */
public class Player : MonoBehaviour
{
    private AnimationComponent animationComponent;
    public Rigidbody Rigidbody;

    public Transform Head;
    public Transform RightHand;
    public Transform LeftHand;

    public PlayerStatesBlackboard Blackboard => blackboard;

    private StateMachine<PlayerStates, Events> fsmRoot;
    private StateMachine<PlayerStates, MovementStates, Events> fsmMovement;
    private StateMachine<PlayerStates, CombatStates, Events> fsmCombat;
    private StateMachine<CombatStates, WeaponStates, WeaponEvents> fsmWeaponed;
    private Weapon currentWeapon;

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
    [SerializeField] private AvatarMask footAvatarMask;
    [SerializeField] private AvatarMask handAttackAvatarMask;
    [SerializeField] private AvatarMask totalAvatarMask;

    private float idleSpeed = 0f;
    private float walkSpeed = 1f;
    private float extraSpeed = 0f;
    private float baseSpeed = 0f;

    private bool isReady = false;
    public bool ApplyAnimatorIK
    {
        get => animationComponent.Animancer.Layers[PlayerAnimationLayer.Base].ApplyAnimatorIK;
        set => animationComponent.Animancer.Layers[PlayerAnimationLayer.Base].ApplyAnimatorIK = value;
    }
    #region Unity
    private void Awake()
    {
        animationComponent = GetComponent<AnimationComponent>();
        Rigidbody = GetComponent<Rigidbody>();

        leftFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        footWeights = new AnimatedFloat(animationComponent.Animancer, "LeftFootWeightCurve", "RightFootWeightCurve");
        ApplyAnimatorIK = true;

        blackboard = new PlayerStatesBlackboard();
        blackboard.Player = this;

        animationComponent.Animancer.Layers[PlayerAnimationLayer.Action].SetDebugName("Action Layer");
        animationComponent.Animancer.Layers[PlayerAnimationLayer.Action].SetMask(totalAvatarMask);
        animationComponent.Animancer.Layers[PlayerAnimationLayer.Base].SetMask(totalAvatarMask);

        animationComponent.Animancer.Layers[PlayerAnimationLayer.HandAttack].SetDebugName("HandAttack Layer");
        animationComponent.Animancer.Layers[PlayerAnimationLayer.HandAttack].SetMask(handAttackAvatarMask);
    }

    private void Start()
    {
        CreateHFSM();
        isReady = true;
    }

    private void Update()
    {
        if (isReady)
        {
            fsmMovement.OnLogic();
            fsmCombat.OnLogic();
        }

    }
    #endregion

    private void CreateHFSM()
    {
        fsmRoot = new StateMachine<PlayerStates, Events>();

        fsmMovement = new StateMachine<PlayerStates, MovementStates, Events>();

        /* -----------------------------InGround-------------------------*/
        var fsmInGround = new StateMachine<MovementStates, InGroundStates, Events>();
        fsmInGround.AddState(InGroundStates.Idle, new PlayerIdleState(blackboard, false, false));
        fsmInGround.AddState(InGroundStates.Walk, new PlayerWalkState(blackboard, false, false));
        fsmInGround.AddState(InGroundStates.Dash, new PlayerDashState(blackboard, false, false));
        fsmInGround.SetStartState(InGroundStates.Idle);

        fsmInGround.AddTwoWayTransition(new Transition<InGroundStates>(InGroundStates.Walk, InGroundStates.Dash, condition: GroundWalkToDashCondition));
        fsmInGround.AddTwoWayTransition(new Transition<InGroundStates>(InGroundStates.Idle, InGroundStates.Walk, condition: GroundIdleToWalkCondition));
        fsmInGround.AddTwoWayTransition(new Transition<InGroundStates>(InGroundStates.Idle, InGroundStates.Dash, condition: GroundIdleToDashCondition));

        /* -----------------------------InSky-------------------------*/
        var fsmInSky = new StateMachine<MovementStates, InSkyStates, Events>();
        fsmInSky.AddState(InSkyStates.Jump, new PlayerJumpState(blackboard, false, false));

        fsmMovement.AddState(MovementStates.InGround, fsmInGround);
        fsmMovement.AddState(MovementStates.InSky, fsmInSky);
        fsmMovement.SetStartState(MovementStates.InGround);
        /*----------------------------Combat-------------------------*/
        fsmCombat = new StateMachine<PlayerStates, CombatStates, Events>();

        /*---------------------------Weaponed-------------------------*/
        fsmWeaponed = new StateMachine<CombatStates, WeaponStates, WeaponEvents>();
        fsmWeaponed.AddState(WeaponStates.Idle, new PlayerAttackIdleState(blackboard, false, false));
        fsmWeaponed.AddState(WeaponStates.L1, new PlayerAttackL1State(blackboard, false, false));
        fsmWeaponed.AddState(WeaponStates.R1, new PlayerAttackR1State(blackboard, false, false));
        fsmWeaponed.AddState(WeaponStates.L1R1, new PlayerAttackL1R1State(blackboard, false, false));
        fsmWeaponed.SetStartState(WeaponStates.Idle);

        //fsmWeaponed.AddTriggerTransition(WeaponEvents.L1, new Transition<WeaponStates>(WeaponStates.Idle, WeaponStates.L1, condition: FromIdleAttackCondition));
        //fsmWeaponed.AddTriggerTransition(WeaponEvents.R1, new Transition<WeaponStates>(WeaponStates.Idle, WeaponStates.R1, condition: FromIdleAttackCondition));
        //fsmWeaponed.AddTriggerTransition(WeaponEvents.L1R1, new Transition<WeaponStates>(WeaponStates.L1, WeaponStates.L1R1, condition: FromL1AttackCondition));
        fsmCombat.AddState(CombatStates.Weaponed, fsmWeaponed);

        fsmCombat.SetStartState(CombatStates.Weaponed);

        //fsmRoot.AddState(PlayerStates.Movement, fsmMovement);
        //fsmRoot.AddState(PlayerStates.Combat, fsmCombat);
        //fsmRoot.AddState(PlayerStates.Jump, new State<PlayerStates>(onEnter: OnEnterJumpState));

        //var moveFsm = new StateMachine<MovementStates, MoveStates, Events>();
        //var idleFsm = new StateMachine<MovementStates, IdleStates, Events>();
        // IDLE
        //idleFsm.AddState(IdleStates.BASE, new State<IdleStates, Events>(onEnter: OnEnterBaseIdle));
        //idleFsm.SetStartState(IdleStates.BASE);

        // MOVE
        //moveFsm.AddState(MoveStates.WALK, new PlayerWalkState(blackboard, false, false));
        //moveFsm.AddState(MoveStates.DASH, new PlayerDashState(blackboard, false, false));

        // Transition
        //moveFsm.AddTransition(new Transition<MoveStates>(MoveStates.WALK, MoveStates.DASH, condition: WalkToDashCondition));
        //moveFsm.AddTransition(new Transition<MoveStates>(MoveStates.DASH, MoveStates.WALK, condition: DashToWalkCondition));

        // IDLE ->MOVE
        //fsmRoot.AddTransition(new Transition<PlayerStates>(PlayerStates.Move, PlayerStates.Idle, condition: MoveToIdleCondition));
        // MOVE ->IDLE
        //fsmRoot.AddTransition(new Transition<PlayerStates>(PlayerStates.Idle, PlayerStates.Move, condition: IdleToMoveCondition));
        // Any -> Death
        //fsmRoot.AddTriggerTransition(Events., transition);

        //fsmRoot.SetStartState(PlayerStates.Idle);
        //fsmRoot.Init()
        fsmMovement.Init();
        fsmCombat.Init();
    }

    private void OnEnterBaseIdle(State<IdleStates, Events> state)
    {
        Debug.Log("OnEnterBaseIdle");
        PlayAnimation(PlayerAnimationLayer.Base, AnimationType.Idle, 1f, FadeMode.FromStart);
    }

    private void OnEnterJumpState(State<PlayerStates, string> state)
    {
        var animancerState = PlayAnimation(PlayerAnimationLayer.Action, AnimationType.Jump, 1f, FadeMode.FromStart, OnJumpEnd);
        //animancerState.SetWeight(0.5f);
    }

    private void OnJumpEnd(AnimancerState animancerState)
    {
        animancerState.Layer.StartFade(0, 0);
        SwitchState(PlayerStates.Idle);
    }

    #region FsmCondition
    private bool GroundWalkToDashCondition(Transition<InGroundStates> groundStateTransition)
    {
        return blackboard.MoveSpeed > walkSpeed;
    }

    private bool GroundIdleToWalkCondition(Transition<InGroundStates> groundStateTransition)
    {
        return blackboard.MoveSpeed > idleSpeed && blackboard.MoveSpeed <= walkSpeed;
    }

    private bool GroundIdleToDashCondition(Transition<InGroundStates> groundStateTransition)
    {
        return blackboard.MoveSpeed > idleSpeed && blackboard.MoveSpeed > walkSpeed;
    }

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

    private bool FromIdleAttackCondition(Transition<WeaponStates> transition)
    {
        return fsmWeaponed.ActiveStateName == WeaponStates.Idle;
    }

    private bool FromL1AttackCondition(Transition<WeaponStates> transition)
    {
        return fsmWeaponed.ActiveStateName == WeaponStates.L1;
    }
    #endregion

    public AnimancerState PlayAnimation(int layer, AnimationType animationType, float speed, FadeMode fadeMode=default, Action<AnimancerState> onEnd=null)
    {
         return animationComponent.Play(layer, animationType, speed, fadeMode, onEnd);
    }

    public void StopAnimation(int layer)
    {
        animationComponent.Stop(layer);
    }

    private void SwitchState(PlayerStates playerState)
    {
        fsmRoot.RequestStateChange(playerState, true);
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
        if (dir.x != 0 && dir.y != 0)
        {
            if (dir.y > 0 && dir.x < 0) // leftForward
            {
                //anim.SetTrigger("move_up_left");
                //targetEulerAngles = new Vector3(0, -45, 0);
                targetForward = GetTargetForward(-45);
            }
            if (dir.y > 0 && dir.x > 0) // rightForward
            {
                //anim.SetTrigger("move_up_right");
                //targetEulerAngles = new Vector3(0, 45, 0);
                targetForward = GetTargetForward(45);
            }
            if (dir.y < 0 && dir.x < 0) // backleft
            {
                //anim.SetTrigger("move_down_left");
                //targetEulerAngles = new Vector3(0, -135, 0);
                targetForward = GetTargetForward(-135);
            }
            if (dir.y < 0 && dir.x > 0) // backright
            {
                //anim.SetTrigger("move_down_right");
                //targetEulerAngles = new Vector3(0, 135, 0);
                targetForward = GetTargetForward(135);
            }
        }

        else
        {
            //left/right/up/down
            if (dir.x < 0)
            {
                //targetForward = Vector3.left;
                targetForward = GetTargetForward(-90);
                //anim.SetTrigger("move_left");
            }
            if (dir.x > 0)
            {
                targetForward = GetTargetForward(90);
                //anim.SetTrigger("move_right");
            }
            if (dir.y > 0)
            {
                targetForward = GetTargetForward(0);
                //anim.SetTrigger("move_up");
            }
            if (dir.y < 0)
            {
                targetForward = GetTargetForward(180);
                //anim.SetTrigger("move_down");
            }
        }
        blackboard.TargetForward = targetForward;
        blackboard.MoveInput = dir;
        if (dir.magnitude > 0)
        {
            blackboard.BaseSpeed = walkSpeed;
        }
        else
        {
            blackboard.BaseSpeed = idleSpeed;
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
        //Debug.Log("OnAnimatorMove:" + isReady);
        if (!isReady) return;
        //animationComponent.Animancer.Animator.ApplyBuiltinRootMotion();
        // Rigidbody
        if (fsmMovement.ActiveState != null)
        {
            switch (fsmMovement.ActiveStateName)
            {
                case MovementStates.InGround:
                    if (blackboard.TargetForward != Vector3.zero)
                    {
                        transform.forward = blackboard.TargetForward;
                    }
 
                    Rigidbody.MovePosition(Rigidbody.position + animationComponent.Animancer.Animator.deltaPosition);
                    break;
                case MovementStates.InSky:
                    transform.forward = Vector3.RotateTowards(transform.forward, blackboard.TargetForward, 2f * Time.deltaTime, 0.0f);
                    Rigidbody.MovePosition(Rigidbody.position + blackboard.TargetForward * Time.deltaTime * 5f * Blackboard.MoveInput.magnitude);
                    break;
            }
        }
        

        //Rigidbody.MoveRotation(Rigidbody.rotation * animationComponent.Animancer.Animator.deltaRotation);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + targetForward * blackboard.MoveInput.magnitude);
        }
 
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
        if (isAccelerate)
        {
            blackboard.ExtralSpeed = walkSpeed;
        }
        else
        {
            blackboard.ExtralSpeed -= walkSpeed;
        }
    }

    #endregion

    #region Jump
    public bool TryEnterJumpState()
    {
        if (fsmRoot.ActiveStateName != PlayerStates.Jump)
        {
            SwitchState(PlayerStates.Jump);
            return true;
        }
        return false;
    }
    #endregion

    #region Weapon
    public void AddWeapon(Weapon weaponPrefab)
    {
        currentWeapon = Instantiate(weaponPrefab, RightHand, false);
        currentWeapon.transform.localEulerAngles = new Vector3(80, 0, 0);
        currentWeapon.transform.localPosition = new Vector3(0.085f, 0.016f, 0.503f);
    }
    #endregion

    public void ExecuteCommand(CommandType commandType)
    {
        switch (commandType)
        {
            case CommandType.LeftAttack:
            case CommandType.RightAttack:
                OnMouseClickEvent(commandType);
                break;
        }
    }

    private void OnMouseClickEvent(CommandType commandType)
    {
        switch (fsmWeaponed.ActiveStateName)
        {
            case WeaponStates.Idle:
                if (commandType == CommandType.LeftAttack)
                {
                    fsmWeaponed.RequestStateChange(WeaponStates.L1);
                }
                else if (commandType == CommandType.RightAttack)
                {
                    fsmWeaponed.RequestStateChange(WeaponStates.R1);
                }
                break;
            case WeaponStates.L1:
                if (commandType == CommandType.RightAttack)
                {
                    fsmWeaponed.RequestStateChange(WeaponStates.L1R1);
                }
                break;
        }
    }
}


public enum PlayerStates
{
    Idle,
    Move,
    Jump,
    Attack,

    Movement,
    Combat,
    Death,
}

public enum MoveStates
{
    WALK, DASH
}

public enum MovementStates
{
    InGround,
    InSky,
}

public enum InGroundStates
{
    Idle,
    Walk,
    Dash,
}

public enum InSkyStates
{
    Jump,
    Down,
}

public enum CombatStates
{
    Unarmed,    //徒手
    Weaponed,   //装备
}

public enum WeaponStates
{
    Idle,
    L1,
    R1,
    L1R1,
}


public enum IdleStates
{
    BASE,
}

enum Events
{
    ON_DAMAGE,
    ON_WIN,
}

public enum WeaponEvents
{
    L1,
    R1,
    L1R1,
}

public static class PlayerAnimationLayer
{
    public static int Base = 0;
    public static int Action = 1;
    public static int HandAttack = 2;
}

public class PlayerStatesBlackboard
{
    public Player Player { get; set; }
    public bool IsAccelerate { get; set; }
    public Vector2 MoveInput { get; set; }
    public Vector3 TargetForward { get; set; }
    public bool IsLeftClick { get; set; }
    public bool IsRightClick { get; set; }
    public float BaseSpeed = 0f;
    public float ExtralSpeed = 0f;
    public float MoveSpeed => BaseSpeed + ExtralSpeed;
}
