using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

[RequireComponent(typeof(PlayerInput))]
public class GamePlayInput : MonoBehaviour
{
    PlayerInput playerInput;
    CommandInvoker commandInvoker;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        commandInvoker = Game.Instance.GetGameSystem<CommandInvoker>();
    }

    public void OnJumpEvent(InputAction.CallbackContext context)
    {
        commandInvoker.AddCommand(new JumpCommand());
    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
    }
}
