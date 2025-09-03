using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GamePlayerInput : MonoBehaviour
{
    // 输入支持的事件
    public UnityAction<ICommand> JumpAction;
    public UnityAction<ICommand> MoveAction;
    public UnityAction<ICommand> AccelerateAction;
    public UnityAction<ICommand> AttackAction;
    public UnityAction<ICommand> SkillCastAction;

    private PlayerInput m_PlayerInput;

    public PlayerInput PlayerInput => m_PlayerInput;

    private bool m_CanAcceptInput;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeGame()
    {
        m_CanAcceptInput = true;
    }

    public void SetCanAcceptInput(bool canAcceptInput)
    {
        m_CanAcceptInput = canAcceptInput;
    }

    public void OnJumpEvent(InputAction.CallbackContext context)
    {
        if (m_CanAcceptInput && context.started)
        {
            JumpAction?.Invoke(new JumpCommand());
        }

    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
        if (m_CanAcceptInput)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            MoveCommand moveCommand = new MoveCommand();
            moveCommand.MoveDir = dir;
            MoveAction?.Invoke(moveCommand);
        }
    }

    public void OnAccelerateEvent(InputAction.CallbackContext context)
    {
        if (!m_CanAcceptInput)
        {
            return;
        }

        if (context.started)
        {
            AccelerateCommand accelerateCommand = new AccelerateCommand();
            accelerateCommand.Value = true;
            AccelerateAction?.Invoke(accelerateCommand);
        }
        else if (context.canceled)
        {
            AccelerateCommand accelerateCommand = new AccelerateCommand();
            accelerateCommand.Value = false;
            AccelerateAction?.Invoke(accelerateCommand);
        }
    }

    public void OnLeftClickEvent(InputAction.CallbackContext context)
    {
        if (m_CanAcceptInput && context.started)
        {
            AttackCommand command = new AttackCommand();
            command.LeftClick = true;
            command.RightClick = false;
            AttackAction?.Invoke(command);
        }
    }

    public void OnRightClickEvent(InputAction.CallbackContext context)
    {
        if (m_CanAcceptInput && context.started)
        {
            AttackCommand command = new AttackCommand();
            command.LeftClick = false;
            command.RightClick = true;
            AttackAction?.Invoke(command);
        }
    }

    public void OnSkill1ClickEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SendSkillCommand(0);
        }
    }

    public void OnSkill2ClickEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SendSkillCommand(1);
        }
    }

    public void OnSkill3ClickEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SendSkillCommand(2);
        }
    }

    public void OnSkill4ClickEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SendSkillCommand(3);
        }
    }

    private void SendSkillCommand(int skillSlot)
    {
        if (m_CanAcceptInput)
        {
            SkillCommand command = new SkillCommand();
            command.SkillSlot = skillSlot;
            SkillCastAction?.Invoke(command);
        }
    }
}
