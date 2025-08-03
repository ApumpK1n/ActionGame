using UnityEngine;

public interface ICharacterView
{
    /// <summary>
    /// 角色Transform
    /// </summary>
    Transform Transform { get; }

    /// <summary>
    /// 相机指向的点
    /// </summary>
    Transform LookAtPoint { get; }

    void OnBind(Character logicCharacter);

    void AddCharacterParent(Transform parent);

    void OnAddToSceneView(SceneViewLogic sceneViewLogic);

    void OnSetup();

    void Move(Vector2 direction);

    void SetAccelerate(bool accelerate);

    void ExecuteCommand(CommandType commandType);

    void PerformSkill(int skillSlot);
}
