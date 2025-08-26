using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteRoleStatusBar : MonoBehaviour
{
    [SerializeField] Transform healthBar;
    [SerializeField] Vector3 posOffset = new Vector3(0, 2.2f, 0);

    Camera mainCamera;
    Transform targetRole;
    float followSpeed = 0.25f;

    //记录初始位置
    float healthBarInitPosX;
    Vector3 healthInitScale;

    void Awake()
    {
        healthBarInitPosX = healthBar.localPosition.x;
        healthInitScale = transform.localScale;
    }

    void Update()
    {
        if (targetRole != null)
            UpdateBarPos();
    }

    public void Init(Transform role, Camera camera)
    {
        mainCamera = camera;
        targetRole = role;
        UpdateBarPos();
    }

    Vector3 healthBarPos = Vector3.zero;
    Vector3 healthBarScale = Vector3.one;
    public void OnHealthChanged(float healthPercent)
    {
        if (healthPercent < 0)
            healthPercent = 0;
        if (healthPercent > 1)
            healthPercent = 1;


        //修改位置和缩放，实现血条效果
        //healthBarPos.x = healthBarInitPosX * (1 - healthPercent);
        //healthBar.localPosition = healthBarPos;

        healthBarScale = new Vector3(healthPercent * healthInitScale.x, healthInitScale.y, healthInitScale.z);
        healthBar.localScale = healthBarScale;
    }

    void UpdateBarPos()
    {
        //刷新朝向，始终朝向相机
        //transform.forward = mainCamera.transform.forward;
        //刷新位置
        //transform.position = Vector3.Lerp(transform.position, targetRole.transform.position + posOffset, followSpeed);
    }

}
