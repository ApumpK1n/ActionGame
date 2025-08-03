using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景必要数据及相关逻辑
///
/// 关卡中必须要有这个
/// </summary>
public class SceneViewLogic : MonoBehaviour
{
    /// <summary>
    /// 角色父节点
    /// </summary>
    [SerializeField] private Transform PlayerReborn;
    [SerializeField] private CameraViewInfo CameraView;

    [SerializeField] private Animator debugPlayerCombatAnimator;
    [SerializeField] private Animator debugPlayerMovementAnimator;

    public Animator DebugPlayerCombatAnimator { get { return debugPlayerCombatAnimator; } }
    public Animator DebugPlayerMovementAnimator { get { return debugPlayerMovementAnimator; } }

    public CameraViewInfo CameraViewInfo { get { return CameraView; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 角色加入场景中【表现层】
    /// </summary>
    /// <param name="characterView"></param>
    public void AddPlayerTo(ICharacterView characterView)
    {
        characterView.AddCharacterParent(PlayerReborn);
        characterView.OnAddToSceneView(this);
    }

    public void LookAtCharacter(ICharacterView characterView)
    {
        CameraView.CameraFreeLook.Follow = characterView.Transform;
        CameraView.CameraFreeLook.LookAt = characterView.LookAtPoint;
    }
}
