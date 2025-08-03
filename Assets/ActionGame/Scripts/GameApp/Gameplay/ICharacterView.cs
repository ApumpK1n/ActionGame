using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterView
{
    void OnBind(Character logicCharacter);

    void AddCharacterParent(Transform parent);

    void OnAddToSceneView(SceneViewLogic sceneViewLogic);

    void OnSetup();

    void Move(Vector2 direction);

    void SetAccelerate(bool accelerate);

    void ExecuteCommand(CommandType commandType);

    void PerformSkill(int skillSlot);
}
