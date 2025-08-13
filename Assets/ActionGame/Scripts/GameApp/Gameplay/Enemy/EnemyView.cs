using UnityEngine;

public class EnemyView : MonoBehaviour, ICharacterView
{
    [SerializeField] private Transform healthBarPoint;

    public Transform Transform
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public Transform LookAtPoint
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public void AddCharacterParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;


    }

    public void ExecuteCommand(CommandType commandType)
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public void OnAddToSceneView(SceneViewLogic sceneViewLogic)
    {
        
    }

    public void OnBind(Character logicCharacter)
    {
        throw new System.NotImplementedException();
    }

    public void OnSetup()
    {
        throw new System.NotImplementedException();
    }

    public void PerformSkill(int skillSlot)
    {
        throw new System.NotImplementedException();
    }

    public void SetAccelerate(bool accelerate)
    {
        throw new System.NotImplementedException();
    }
}
