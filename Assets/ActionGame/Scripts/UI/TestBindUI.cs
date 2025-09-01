
using CrashKonijn.Goap.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestBindUI : MonoBehaviour
{
    [SerializeField] Button btnBind;
    [SerializeField] private TextMeshProUGUI textBind;
    [SerializeField] private string textBindActionName;


    int bindingIndex = 0;
    private void Awake()
    {
        btnBind.onClick.AddListener(TryBind);
        GameApp.Instance.GetSubsystem<GameInputSystem>().LoadBindingOverride(GameApp.Instance.GamePlayerInput.PlayerInput.actions, textBindActionName);
        textBind.text = GameApp.Instance.GetSubsystem<GameInputSystem>().GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, textBindActionName, bindingIndex);
        
    }

    private void OnDestroy()
    {
        btnBind.onClick.RemoveAllListeners();
    }

    private void TryBind()
    {
        InputAction inputAction = GameApp.Instance.GamePlayerInput.PlayerInput.actions[textBindActionName];


        GameApp.Instance.GetSubsystem<GameInputSystem>().StartRebind(GameApp.Instance.GamePlayerInput.PlayerInput.actions,
            textBindActionName, bindingIndex, true, new GameInputSystem.RebindActions()
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
        textBind.text = GameApp.Instance.GetSubsystem<GameInputSystem>().GetBindingName(GameApp.Instance.GamePlayerInput.PlayerInput.actions, textBindActionName, bindingIndex);
        GameApp.Instance.GetSubsystem<GameInputSystem>().LoadBindingOverride(GameApp.Instance.GamePlayerInput.PlayerInput.actions, textBindActionName);
    }

}
