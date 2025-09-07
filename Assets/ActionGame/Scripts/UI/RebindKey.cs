using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindKey : MonoBehaviour
{
    [SerializeField] private Button btnChangeKey;
    [SerializeField] private TextMeshProUGUI textKeyName;
    [SerializeField] private TextMeshProUGUI textKey;
    [SerializeField] private TextMeshProUGUI textTip;

    private RebindKeys rebindKey;
    private Mode mode = Mode.Normal;

    public int BindingIndex = 0;

    public enum Mode
    {
        Normal,
        Bind,
    }

    void Start()
    {
        btnChangeKey.onClick.AddListener(OnEnterRebindKey);
        btnChangeKey.onClick.AddListener(OnEnterRebindKey);
    }

    private void OnDestroy()
    {
        btnChangeKey.onClick.RemoveListener(OnEnterRebindKey);
    }

    public void SetData(RebindKeys rebindKey)
    {
        this.rebindKey = rebindKey;
        textKeyName.text =$"{rebindKey}:";
        mode = Mode.Normal;
        UpdateViewByMode();
    }

    private void OnEnterRebindKey()
    {
        mode = Mode.Bind;
        UpdateViewByMode();
    }

    private void UpdateViewByMode()
    {
        
        switch (mode)
        {
            case Mode.Normal:
                textTip.gameObject.SetActive(false);
                textKey.gameObject.SetActive(true);
                UpdateKeyName();
                break;
            case Mode.Bind:
                textTip.gameObject.SetActive(true);
                textKey.gameObject.SetActive(false);
                break;
        }
    }

    private void UpdateKeyName()
    {
        GameInputSystem gameInputSystem = GameApp.Instance.GetSubsystem<GameInputSystem>();
        string keyName = "";
        switch (rebindKey)
        {
            case RebindKeys.Up:
                BindingIndex = 1;
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, "Move", 1);
                break;
            case RebindKeys.Down:
                BindingIndex = 2;
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, "Move", 2);
                break;
            case RebindKeys.Left:
                BindingIndex = 3;
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, "Move", 3);
                break;
            case RebindKeys.Right:
                BindingIndex = 4;
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, "Move", 4);
                break;
            default:
                BindingIndex = 0;
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, rebindKey.ToString(), 0);
                break;
        }

        textKey.text = keyName;
    }

    private string GetActionNameByBindKey()
    {
        string name = "";
        switch (rebindKey)
        {
            case RebindKeys.Up:
            case RebindKeys.Down:
            case RebindKeys.Left:
            case RebindKeys.Right:
                name = "Move";
                break;
            default:
                name = rebindKey.ToString();
                break;
        }

        return name;
    }

    private void TryBind()
    {
        string actionName = GetActionNameByBindKey();
        InputAction inputAction = GameApp.Instance.GamePlayerInput.PlayerInput.actions[actionName];


        GameApp.Instance.GetSubsystem<GameInputSystem>().StartRebind(GameApp.Instance.GamePlayerInput.PlayerInput.actions,
            actionName, this.BindingIndex, true, new GameInputSystem.RebindActions()
            { RebindCanceled = OnRebindCanceled, RebindCompleted = OnRebindCompleted, RebindStarted = OnRebindStarted });

    }

    private void OnRebindStarted(InputAction inputAction, int bindingIndex)
    {
        Debug.Log($"开始绑定{inputAction.name}, {bindingIndex}");
        Debug.Log("请按键");
    }

    private void OnRebindCanceled()
    {
        Debug.Log("取消绑定");
    }

    private void OnRebindCompleted()
    {
        Debug.Log("绑定完成");
        UpdateViewByMode();
    }
}
