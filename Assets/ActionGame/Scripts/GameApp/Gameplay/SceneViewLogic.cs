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
    }
}
