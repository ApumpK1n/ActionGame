using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: 指令需要改成结构体或者复用 目前堆上创建对象太频繁

[RequireComponent(typeof(PlayerInput))]
public class GamePlayInput : MonoBehaviour
{
    PlayerInput playerInput;
    CommandInvoker commandInvoker;
    private void Start()
    {
        //InputSystem.settings.SetInternalFeatureFlag("DISABLE_SHORTCUT_SUPPORT", true);
        playerInput = GetComponent<PlayerInput>();
        commandInvoker = Game.Instance.GetGameSystem<CommandInvoker>();
    }

    public void OnJumpEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            commandInvoker.AddCommand(new JumpCommand());
        }

    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        MoveCommand moveCommand = new MoveCommand();
        moveCommand.MoveDir = dir;
        commandInvoker.AddCommand(moveCommand);
    }

    public void OnAccelerateEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AccelerateCommand accelerateCommand = new AccelerateCommand();
            accelerateCommand.Value = true;
            commandInvoker.AddCommand(accelerateCommand);
        }
        else if (context.canceled)
        {
            AccelerateCommand accelerateCommand = new AccelerateCommand();
            accelerateCommand.Value = false;
            commandInvoker.AddCommand(accelerateCommand);
        }
    }

    public void OnLeftClickEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackCommand command = new AttackCommand();
            command.LeftClick = true;
            command.RightClick = false;
            commandInvoker.AddCommand(command);
        }
    }

    public void OnRightClickEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackCommand command = new AttackCommand();
            command.LeftClick = false;
            command.RightClick = true;
            commandInvoker.AddCommand(command);
        }
    }

    public void OnSkill1ClickEvent(InputAction.CallbackContext context)
    {
        Debug.Log("OnSkill1ClickEvent");
        if (context.started)
        {
            SkillCommand command = new SkillCommand();
            command.SkillSlot = 0;

            commandInvoker.AddCommand(command);
        }
    }
}
