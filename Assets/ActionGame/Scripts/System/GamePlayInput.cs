using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

[RequireComponent(typeof(PlayerInput))]
public class GamePlayInput : MonoBehaviour
{
    PlayerInput playerInput;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnJumpEvent(InputAction.CallbackContext context)
    {
        MessageBroker.Default.Publish(new GamePlayJumpLongEvent());
    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
    }
}
