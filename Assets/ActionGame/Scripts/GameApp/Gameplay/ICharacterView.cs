using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterView
{
    void AddCharacterParent(Transform parent);

    void OnAddToSceneView(SceneViewLogic sceneViewLogic);

    void OnSetup();
}
