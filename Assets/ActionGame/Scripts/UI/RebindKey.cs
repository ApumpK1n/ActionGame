using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindKey : MonoBehaviour
{
    [SerializeField] private Button btnChangeKey;
    [SerializeField] private TextMeshProUGUI textKey;
    [SerializeField] private TextMeshProUGUI textTip;

    private RebindKeys rebindKey;
    private Mode mode = Mode.Normal;

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
        textKey.name = rebindKey.ToString();
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
            case RebindKeys.Down:
            case RebindKeys.Left:
            case RebindKeys.Right:
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, "Move", 1);
                break;
            default:
                keyName = gameInputSystem.GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, rebindKey.ToString(), 0);
                break;
        }

        textKey.name = keyName;
    }
}
