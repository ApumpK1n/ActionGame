using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInputSystem : IGameAppSubsystem
{
    public SystemType TypeEnum
    {
        get
        {
            return SystemType.GameInput;
        }
    }
    #region SubSystem
    public IGameApp GetGameApp()
    {
        throw new NotImplementedException();
    }

    public void Setup()
    {

    }

    public void Start()
    {

    }

    public void Tick(float deltaTime)
    {

    }

    public void Dispose()
    {

    }
    #endregion

    public struct RebindActions
    {
        public Action<InputAction, int> RebindStarted;
        public Action RebindCompleted;
        public Action RebindCanceled;
    }

    public void StartRebind(InputActionAsset inputActionAsset, string actionName, int bindingIndex, bool excludeMouse, RebindActions rebindActions)
    {
        InputAction action = inputActionAsset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couldn't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                DoRebind(action, bindingIndex, true, excludeMouse, rebindActions);
        }
        else
            DoRebind(action, bindingIndex, false, excludeMouse, rebindActions);
    }

    private void DoRebind(InputAction actionToRebind, int bindingIndex, bool allCompositeParts, bool excludeMouse, RebindActions rebindActions)
    {
        if (actionToRebind == null || bindingIndex < 0)
            return;

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, allCompositeParts, excludeMouse, rebindActions);
            }

            SaveBindingOverride(actionToRebind);
            rebindActions.RebindCompleted?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindActions.RebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");

        if (excludeMouse)
            rebind.WithControlsExcluding("Mouse");

        rebindActions.RebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start(); //actually starts the rebinding process
    }

    public string GetBindingName(InputActionAsset inputActionAsset, string actionName, int bindingIndex)
    {
        InputAction action = inputActionAsset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    private void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }


    /// <summary>
    /// TODO: 先用PlayerPrefs 后面改成json文件
    /// </summary>
    /// <param name="gameInput"></param>
    /// <param name="actionName"></param>
    public void LoadBindingOverride(InputActionAsset inputActionAsset, string actionName)
    {
        InputAction action = inputActionAsset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
        }
    }

    public void ResetBinding(InputActionAsset inputActionAsset, string actionName, int bindingIndex)
    {
        InputAction action = inputActionAsset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Could not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                action.RemoveBindingOverride(i);
        }
        else
            action.RemoveBindingOverride(bindingIndex);

        SaveBindingOverride(action);
    }

}
