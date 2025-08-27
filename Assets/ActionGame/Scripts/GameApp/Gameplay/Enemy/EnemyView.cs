using System.Drawing;
using CombatAbilitySystem;
using UnityEngine;
using System.Collections.Generic;
using DamageNumbersPro;

public class EnemyView : MonoBehaviour, ICharacterView, IAbilityApplyComponent
{
    [SerializeField] private Transform healthBarPoint;
    [SerializeField] private List<AttributeConfig> attributeConfigs;
    [SerializeField] DamageNumber damageNumberPrefab;
    private CameraViewInfo cameraViewInfo;

    private HealthBar healthBar;
    private AbilitySystemComponent AbilitySystem;

    private float baseHealth = 100;

    private SpriteRoleStatusBar spriteHealthBar;
    private AttributeConfig healthConfig;

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

    private void Awake()
    {
        foreach (var attribute in attributeConfigs)
        {
            if (attribute.Name == "Hp")
            {
                healthConfig = attribute;
            }

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

        spriteHealthBar = GetComponentInChildren<SpriteRoleStatusBar>();
        spriteHealthBar.Init(this.healthBarPoint, cameraViewInfo.MainCamera);
        //Vector3 viewPoint = cameraViewInfo.MainCamera.WorldToViewportPoint(healthBarPoint.position);
        //healthBar = WorldCanvasController.Instance.AddHealthBar();
        //UpdateHealthBarPosition();

        SetupAbilitySystem();
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

    public void ApplyGameEffect(AbilityComponent abilityComponent)
    {
        AbilitySystem.TryApplyGameEffect(abilityComponent, 1.0f);

    }

    private void SetupAbilitySystem()
    {
        AbilitySystem = new AbilitySystemComponent(this.gameObject, new EnemyAttributeSet(10));
        AbilitySystem.AttributeChangeAction += HandleAttributeChangeAction;
        AbilitySystem.InitAttributes(attributeConfigs);

        AbilitySystem.AttributeSet.InitBaseValue(healthConfig, baseHealth);
    }

    private void UpdateHealthProgress()
    {
        float currentValue = AbilitySystem.AttributeSet.GetCurrentValue(healthConfig);
        spriteHealthBar.OnHealthChanged(currentValue / 100f);
    }

    private void HandleAttributeChangeAction(AttributeChangeInfo attributeChangeInfo)
    {
        if (attributeChangeInfo.Attribute == healthConfig)
        {
            UpdateHealthProgress();
            DamageNumber newPopup = damageNumberPrefab.Spawn(this.healthBarPoint.position + new Vector3(0, 0.25f, -1), attributeChangeInfo.CurrentValue - attributeChangeInfo.PreValue);
            newPopup.SetFollowedTarget(this.healthBarPoint);
        }
    }
}
