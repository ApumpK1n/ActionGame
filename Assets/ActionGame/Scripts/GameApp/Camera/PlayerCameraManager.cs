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

    private CameraViewInfo m_CameraViewInfo;

    public Camera MainCamera { get { return m_CameraViewInfo.MainCamera; } }

    /// <summary>
    /// 设置控制相机
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public PlayerCameraManager()
    {
    }

    public void SwitchToCamera(CameraViewInfo cameraView)
    {
        m_CameraViewInfo = cameraView;
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
