using System;
using System.Collections.Generic;
using Animancer;
using Animancer.Units;
using CombatAbilitySystem;
using UnityEngine;
using UnityHFSM;
using UnityHFSM.Visualization;

public class CharacterView : MonoBehaviour, ICharacterView
{
    public Rigidbody Rigidbody;

    public Transform Neck;
    public Transform Head;
    public Transform RightHand;
    public Transform LeftHand;
    public Transform LeftFoot;
    public Transform RightFoot;
    public Transform IKFootRoot;
    public Transform IKFootLeft;
    public Transform IKFootRight;

    // 配置
    [SerializeField] CharacterConfig config;

    public PlayerStatesBlackboard Blackboard => blackboard;

    private AnimationComponent animationComponent;

    private StateMachine<PlayerStates, Events> fsmRoot;
    private StateMachine<PlayerStates, MovementStates, Events> fsmMovement;
    private StateMachine<PlayerStates, CombatStates, Events> fsmCombat;
    private StateMachine<CombatStates, WeaponStates, WeaponEvents> fsmWeaponed;
    private StateMachine<MovementStates, InSkyStates, Events> fsmInSky;
    private Weapon currentWeapon;
    private float groundCheckRadius = 1f;
    private bool isGround;

    public enum MoveMode
    {
        Base = 0,
        Lock = 1,
    }


    public Animator DebugCombatAnimator;
    public Animator DebugMovementAnimator;

    private MoveMode moveMode = MoveMode.Base;
    private PlayerStatesBlackboard blackboard;

    public Vector3 targetForward;

    private Transform leftFoot;
    private Transform rightFoot;
    private AnimatedFloat footWeights;
    [SerializeField, Meters] private float _RaycastOriginY = 0.5f;
    [SerializeField, Meters] private float _RaycastEndY = -0.2f;
    [SerializeField] private AvatarMask lowerBodyAvatarMask;
    [SerializeField] private AvatarMask handAttackAvatarMask;
    [SerializeField] private AvatarMask totalAvatarMask;

    private bool isReady = false;
    public bool ApplyAnimatorIK
    {
        get => animationComponent.Animancer.Layers[PlayerAnimationLayer.Base].ApplyAnimatorIK;
        set => animationComponent.Animancer.Layers[PlayerAnimationLayer.Base].ApplyAnimatorIK = value;
    }

    public Transform Transform => transform;

    public Transform LookAtPoint => Neck;

    private PlayerAttribute playerAttribute;
    private WorldScene belongWorldScene;

    private AbilitySystemComponent abilitySystemComponent;

    [SerializeField, Header("技能槽")] private List<AbilityConfig> skillSlots;
    [SerializeField] private List<AttributeConfig> attributeConfigs;

    //
    private Character m_LogicCharacter;

    #region Unity
    private void Awake()
    {
        isReady = false;

        playerAttribute = new PlayerAttribute();
        animationComponent = GetComponent<AnimationComponent>();
        Rigidbody = GetComponent<Rigidbody>();

        leftFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animationComponent.Animancer.Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        footWeights = new AnimatedFloat(animationComponent.Animancer, "LeftFootWeightCurve", "RightFootWeightCurve");
        ApplyAnimatorIK = true;

        blackboard = new PlayerStatesBlackboard();
        blackboard.CharacterView = this;
        blackboard.CharacterConfig = config;

        animationComponent.Animancer.Layers[PlayerAnimationLayer.Base].SetMask(totalAvatarMask);

        animationComponent.Animancer.Layers[PlayerAnimationLayer.LowerBody].SetDebugName("LowerBody Layer");
        animationComponent.Animancer.Layers[PlayerAnimationLayer.LowerBody].SetMask(lowerBodyAvatarMask);

        animationComponent.Animancer.Layers[PlayerAnimationLayer.HandAttack].SetDebugName("HandAttack Layer");
        animationComponent.Animancer.Layers[PlayerAnimationLayer.HandAttack].SetMask(handAttackAvatarMask);
    }


    private void FixedUpdate()
    {
        if (!isReady) return;
        isGround = IsInGround();
    }

    private void Update()
    {
        if (isReady)
        {
            if (!isGround && fsmMovement.ActiveStateName == MovementStates.InGround)
            {
                RequestMovementStateChange(MovementStates.InSky);
                fsmInSky.RequestStateChange(InSkyStates.Down);
            }

            fsmMovement.OnLogic();
            fsmCombat.OnLogic();

#if UNITY_EDITOR
            HfsmAnimatorGraph.PreviewStateMachineInAnimator(fsmMovement, DebugMovementAnimator);
            HfsmAnimatorGraph.PreviewStateMachineInAnimator(fsmCombat, DebugCombatAnimator);
#endif
            abilitySystemComponent.Tick(Time.deltaTime);
        }

    }
    #endregion

    public void OnBind(Character logicCharacter)
    {
        if (logicCharacter == null) return;

        m_LogicCharacter = logicCharacter;
    }

    public void Setup(WorldScene worldScene)
    {
        belongWorldScene = worldScene;
        CreateHFSM();
        CreateCombatAbilitySystem();
        isReady = true;
    }

    private void CreateCombatAbilitySystem()
    {
        abilitySystemComponent = new AbilitySystemComponent(this.gameObject, 10);

        GrandAbilities();
        InitAttributes();
    }

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
        fsmInSky = new StateMachine<MovementStates, InSkyStates, Events>();
        fsmInSky.AddState(InSkyStates.Jump, new PlayerJumpState(blackboard, false, false));
        fsmInSky.AddState(InSkyStates.Down, new PlayerDownState(blackboard, false, false));
        fsmInSky.AddState(InSkyStates.Idle, new PlayerInSkyIdleState(blackboard, false, false));

        fsmInSky.SetStartState(InSkyStates.Idle);

        fsmMovement.AddState(MovementStates.InGround, fsmInGround);
        fsmMovement.AddState(MovementStates.InSky, fsmInSky);
        //fsmMovement.AddTransition(new Transition<MovementStates>(MovementStates.InSky, MovementStates.InGround, condition: InSkyToGroundCondition));
        fsmMovement.SetStartState(MovementStates.InGround);
        /*----------------------------Combat-------------------------*/
        fsmCombat = new StateMachine<PlayerStates, CombatStates, Events>();

        /*---------------------------Weaponed-------------------------*/
        fsmWeaponed = new StateMachine<CombatStates, WeaponStates, WeaponEvents>();
        fsmWeaponed.AddState(WeaponStates.Idle, new PlayerAttackIdleState(blackboard, false, false));
        fsmWeaponed.AddState(WeaponStates.L1, new PlayerAttackL1State(blackboard, false, false));
        fsmWeaponed.AddState(WeaponStates.R1, new PlayerAttackR1State(blackboard, false, false));
        fsmWeaponed.AddState(WeaponStates.L1R1, new PlayerAttackL1R1State(blackboard, false, false));
        fsmWeaponed.AddTransitionFromAny(WeaponStates.Idle, condition: AnyWeaponStateToIdle);
        fsmWeaponed.SetStartState(WeaponStates.Idle);

        fsmCombat.AddState(CombatStates.Weaponed, fsmWeaponed);

        fsmCombat.SetStartState(CombatStates.Weaponed);

        fsmMovement.Init();
        fsmCombat.Init();

#if UNITY_EDITOR
        HfsmAnimatorGraph.CreateAnimatorFromStateMachine(
            fsmMovement,
            outputFolderPath: "Assets/DebugAnimators",
            animatorName: "PlayerMovementStateMachineAnimatorGraph.controller"
        );
        HfsmAnimatorGraph.CreateAnimatorFromStateMachine(
            fsmCombat,
            outputFolderPath: "Assets/DebugAnimators",
            animatorName: "PlayerCombatStateMachineAnimatorGraph.controller"
        );
#endif
    }


    #region FsmCondition

    private bool InSkyToGroundCondition(Transition<MovementStates> transition)
    {
        return fsmMovement.ActiveStateName == MovementStates.InSky && isGround;
    }

    private bool GroundWalkToDashCondition(Transition<InGroundStates> groundStateTransition)
    {
        return blackboard.MoveSpeed > playerAttribute.WalkSpeed;
    }

    private bool GroundIdleToWalkCondition(Transition<InGroundStates> groundStateTransition)
    {
        return blackboard.MoveSpeed > playerAttribute.IdleSpeed && blackboard.MoveSpeed <= playerAttribute.WalkSpeed;
    }

    private bool GroundIdleToDashCondition(Transition<InGroundStates> groundStateTransition)
    {
        return blackboard.MoveSpeed > playerAttribute.IdleSpeed && blackboard.MoveSpeed > playerAttribute.WalkSpeed;
    }

    private bool AnyWeaponStateToIdle(Transition<WeaponStates> transition)
    {
        return blackboard.IsPlayingWeaponAnimation == false;
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


    public AnimancerState PlayAnimation(int layer, AnimationType animationType, float speed, FadeMode fadeMode = default, Action<AnimancerState> onEnd = null)
    {
        return animationComponent.Play(layer, animationType, speed, fadeMode, onEnd);
    }

    public void StopAnimation(int layer)
    {
        animationComponent.Stop(layer);
    }

    public void SetForward(Vector3 forward)
    {
        transform.forward = forward;
    }


    private bool IsInGround()
    {
        LayerMask groundMask = 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Occluder");
        bool isLeftInGround = Physics.Raycast(IKFootLeft.transform.position, Vector3.down, out _, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        bool isRightInGround = Physics.Raycast(IKFootRight.transform.position, Vector3.down, out _, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        return isLeftInGround || isRightInGround;
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
            blackboard.BaseSpeed = playerAttribute.WalkSpeed;
        }
        else
        {
            blackboard.BaseSpeed = playerAttribute.IdleSpeed;
        }

    }

    /// <summary>
    /// get current Camera forward * rotate angle
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 GetTargetForward(float angle)
    {
        if(m_LogicCharacter == null)
        {
            return Vector3.forward;
        }

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
        //Vector3 rotation = quaternion* belongWorldScene.GetPlayerCamera().forward;
        Vector3 rotation = quaternion * m_LogicCharacter.GetController().GetViewForward();
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
                        //transform.forward = blackboard.TargetForward;
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(blackboard.TargetForward), config.MoveTurnSpeed);
                    }

                    Rigidbody.MovePosition(Rigidbody.position + animationComponent.Animancer.Animator.deltaPosition);
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

        if (Application.isPlaying && isReady)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + targetForward * blackboard.MoveInput.magnitude);

            Gizmos.DrawSphere(IKFootRoot.transform.position, groundCheckRadius);

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
            blackboard.ExtralSpeed = playerAttribute.WalkSpeed;
        }
        else
        {
            blackboard.ExtralSpeed -= playerAttribute.WalkSpeed;
        }
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
            case CommandType.Jump:
                OnJump();
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

    private void OnJump()
    {
        switch (fsmMovement.ActiveStateName)
        {
            case MovementStates.InGround: // Start Jump
                SetForward(blackboard.TargetForward);
                //transform.forward = Vector3.RotateTowards(transform.forward, blackboard.TargetForward, 2f * Time.deltaTime, 0.0f);
                Rigidbody.AddForce(new Vector3(200* blackboard.TargetForward.x, 200, 200* blackboard.TargetForward.z), ForceMode.Force);
                RequestMovementStateChange(MovementStates.InSky);
                fsmInSky.RequestStateChange(InSkyStates.Jump);
                break;
        }
    }

    public void RequestMovementStateChange(MovementStates state)
    {
        fsmMovement.RequestStateChange(state);
    }

    #region Ability
    public void GrandAbilities()
    {
        foreach (AbilityConfig abilityConfig in skillSlots)
        {
            if (!abilitySystemComponent.IsGrantedAbility(abilityConfig.Id))
            {
                GrandAbility(abilityConfig);
            }
        }
    }

    private void InitAttributes()
    {
        abilitySystemComponent.InitAttributes(attributeConfigs);
    }

    private AbilityComponent GrandAbility(AbilityConfig abilityConfig)
    {
        AbilityComponent abilityComponent = null;
        switch (abilityConfig)
        {
            case ProjectileAbilityConfig:
                abilityComponent = abilitySystemComponent.GrantAbility<ProjectileAbilityComponent>(abilityConfig);
                break;
            case AreaDamageAbilityConfig:
                abilityComponent = abilitySystemComponent.GrantAbility<AreaDamageAbilityComponent>(abilityConfig);
                break;
            default:
                Debug.LogError($"GrandAbility Error Id:{abilityConfig.Id}, Type:{abilityConfig.GetType()}");
                break;
        }
        return abilityComponent;

    }


    public void PerformSkill(int skillSlot)
    {
        Debug.Log("PerformSkill:" + skillSlot);
        AbilityConfig abilityConfig = skillSlots[skillSlot];
        abilitySystemComponent.TryActivateAbility(abilityConfig.Id);
    }

    public void AddCharacterParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void OnAddToSceneView(SceneViewLogic sceneViewLogic)
    {
        DebugCombatAnimator = sceneViewLogic.DebugPlayerCombatAnimator;
        DebugMovementAnimator = sceneViewLogic.DebugPlayerMovementAnimator;
    }

    public void OnSetup()
    {
        CreateHFSM();
        CreateCombatAbilitySystem();
        isReady = true;
    }

    #endregion
}
