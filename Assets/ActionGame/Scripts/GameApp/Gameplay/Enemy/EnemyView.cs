using System.Drawing;
using UnityEngine;

public class EnemyView : MonoBehaviour, ICharacterView
{
    [SerializeField] private Transform healthBarPoint;
    private CameraViewInfo cameraViewInfo;

    private HealthBar healthBar;
    public Transform Transform
    {
        get
        {
            return this.transform;
        }
    }

    public Transform LookAtPoint
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public void SetCameraViewInfo(CameraViewInfo cameraViewInfo)
    {
        this.cameraViewInfo = cameraViewInfo;
    }


    public void AddCharacterParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        GetComponentInChildren<SpriteRoleStatusBar>().Init(this.transform, cameraViewInfo.MainCamera);
        //Vector3 viewPoint = cameraViewInfo.MainCamera.WorldToViewportPoint(healthBarPoint.position);
        //healthBar = WorldCanvasController.Instance.AddHealthBar();
        //UpdateHealthBarPosition();
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


    private void UpdateHealthBarPosition()
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(this.cameraViewInfo.MainCamera, healthBarPoint.position);
        WorldCanvasController.Instance.UpdateHealthBarPosition(healthBar, screenPosition);

    }

    void Update()
    {
        //UpdateHealthBarPosition();
    }
}
