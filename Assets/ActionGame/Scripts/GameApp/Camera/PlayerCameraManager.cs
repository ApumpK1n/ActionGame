using System;
using UnityEngine;

/// <summary>
/// 管理相机
/// </summary>
public class PlayerCameraManager : Actor
{
    /// <summary>
    /// 所属的PlayerController
    /// </summary>
    private PlayerController m_PCOwner;

    private Camera m_Camera;

    public Camera MainCamera { get { return m_Camera; } }

    /// <summary>
    /// 设置控制相机
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public PlayerCameraManager(Camera camera)
    {
        if(camera == null)
        {
            throw new ArgumentNullException(nameof(camera));
        }

        m_Camera = camera;
    }

    public void InitializeFor(PlayerController pc)
    {
        if(pc == null)
        {
            throw new ArgumentNullException(nameof(pc));
        }

        m_PCOwner = pc;
    }
}
