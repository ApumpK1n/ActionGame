using System;

public enum RebindKeys
{
    Up,
    Down,
    Left,
    Right,
    Jump,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Bag,
}


public static class GameInputKeys
{
    public static string GetActionNameByBindKey(RebindKeys rebindKey)
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

    public static void LoadAllOverrideKeys()
    {
        foreach (RebindKeys key in Enum.GetValues(typeof(RebindKeys)))
        {
            string actionName = GetActionNameByBindKey(key);
            var gameInputSystem = GameApp.Instance.GetSubsystem<GameInputSystem>();
            gameInputSystem.LoadBindingOverride(GameApp.Instance.GamePlayerInput.PlayerInput.actions, actionName);
        }
    }
}
