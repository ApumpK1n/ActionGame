
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// WorldUI的管理 先暂时用个单例 后续根据代码调整
/// </summary>

public class WorldCanvasController : DestroyableSingleton<WorldCanvasController>
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private HealthBar healthBarPrefab;

    public HealthBar AddHealthBar()
    {
        HealthBar healthBar = Instantiate(healthBarPrefab, this.transform);
        return healthBar;
    }

    public void UpdateHealthBarPosition(HealthBar healthBar, Vector2 point)
    {
        //Vector3 position = this.targetCamera.ViewportToWorldPoint(point);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.GetComponent<RectTransform>(), point, targetCamera, out Vector3 worldPoint);
        healthBar.transform.position = worldPoint;// new Vector3(point.x, point.y, 0);
    }
}
