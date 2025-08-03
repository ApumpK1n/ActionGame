using System;
using UnityEngine;

/// <summary>
/// 玩家操控角色控制器
/// </summary>
public class PlayerController : ControllerBase
{
    protected Character m_Character;

    // 输入管理
    protected GamePlayerInput m_PlayerInput;

    // 相机管理
    protected PlayerCameraManager m_PlayerCameraManager;

    private bool m_IsPossessed;

    public PlayerController() : base()
    {
        m_IsPossessed = false;
    }

    public Character SpawnCharacter()
    {
        World world = GetWorld();
        if (world == null)
        {
            throw new NullReferenceException(nameof(World));
        }

        Character character = world.SpawnCharacter();
        character.PossessedBy(this);
        character.Enabled = true;
        character.EnableTick = true;

        return character;
    }

    /// <summary>
    /// 先添加输入
    /// </summary>
    /// <param name="playerInput"> 输入 </param>
    public void InitializePlayerInput(GamePlayerInput playerInput)
    {
        // 输入的初始化
        if(playerInput == null)
        {
            throw new ArgumentNullException(nameof(playerInput));
        }

        m_PlayerInput = playerInput;

        m_PlayerInput.SetCanAcceptInput(true);

        OnInitializePlayerInput();
    }

    public void SpawnPlayerCameraManager()
    {
        m_PlayerCameraManager = new PlayerCameraManager();
        m_PlayerCameraManager.InitializeFor(this);
    }

    public void SwitchToCamera(CameraViewInfo CameraView)
    {
        m_PlayerCameraManager.SwitchToCamera(CameraView);
    }

    public Vector3 GetViewForward()
    {
        return m_PlayerCameraManager.MainCamera.transform.forward;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();

        Clear();
    }

    protected override void OnPossess(Pawn pawn)
    {
        Character characterPawn = pawn as Character;
        if (characterPawn == null)
        {
            throw new ArgumentException(string.Format("cannot conver to Character from: {0}", nameof(pawn)));
        }

        m_Character = characterPawn;

        m_IsPossessed = true;
    }

    protected override void OnUnPossess()
    {
        base.OnUnPossess();

        m_Character = null;
        m_IsPossessed = false;
    }

    private void Clear()
    {
        m_PlayerInput.SetCanAcceptInput(false);
        m_PlayerInput.JumpAction        -= HandleJumpAction;
        m_PlayerInput.MoveAction        -= HandleMoveAction;
        m_PlayerInput.AccelerateAction  -= HandleAccelerateAction;
        m_PlayerInput.AttackAction      -= HandleAttackAction;
        m_PlayerInput.SkillCastAction   -= HandleSkillCastAction;
    }

    // ***************************
    /// <summary>
    /// 输入的初始化
    /// </summary>
    protected virtual void OnInitializePlayerInput()
    {
        m_PlayerInput.JumpAction        += HandleJumpAction;
        m_PlayerInput.MoveAction        += HandleMoveAction;
        m_PlayerInput.AccelerateAction  += HandleAccelerateAction;
        m_PlayerInput.AttackAction      += HandleAttackAction;
        m_PlayerInput.SkillCastAction   += HandleSkillCastAction;
    }

    private void HandleJumpAction(ICommand command)
    {
        if(!m_IsPossessed)
        {
            return;
        }
        m_Character.ExecuteCommand(CommandType.Jump);
    }

    private void HandleMoveAction(ICommand command)
    {
        if (!m_IsPossessed)
        {
            return;
        }

        MoveCommand moveCommand = command as MoveCommand;
        if (moveCommand != null)
        {
            m_Character.Move(moveCommand.MoveDir);
        }
    }

    private void HandleAccelerateAction(ICommand command)
    {
        if (!m_IsPossessed)
        {
            return;
        }

        AccelerateCommand accelerateCommand = command as AccelerateCommand;
        if (accelerateCommand != null)
        {
            m_Character.PlayerAccelerate(accelerateCommand.Value);
        }
    }

    private void HandleAttackAction(ICommand command)
    {
        if (!m_IsPossessed)
        {
            return;
        }

        AttackCommand attackCommand = command as AttackCommand;
        if (attackCommand != null)
        {
            bool isLeftClick = attackCommand.LeftClick;
            bool isRightClick = attackCommand.RightClick;
            if (isLeftClick && isRightClick)
            {
                m_Character.ExecuteCommand(CommandType.BothClick);
            }
            else if (isLeftClick)
            {
                m_Character.ExecuteCommand(CommandType.LeftAttack);
            }
            else if (isRightClick)
            {
                m_Character.ExecuteCommand(CommandType.RightAttack);
            }
        }
    }

    private void HandleSkillCastAction(ICommand command)
    {
        if (!m_IsPossessed)
        {
            return;
        }

        SkillCommand skillCommand = command as SkillCommand;
        if (skillCommand != null)
        {
            m_Character.ExecuteSkillCommand(skillCommand.SkillSlot);
        }
    }
}
