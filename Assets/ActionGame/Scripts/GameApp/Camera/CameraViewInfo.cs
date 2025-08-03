using Cinemachine;
using UnityEngine;

[System.Serializable]
public struct CameraViewInfo
{
    [SerializeField] public Camera MainCamera;
    [SerializeField] public CinemachineBrain CameraBrank;
    [SerializeField] public CinemachineFreeLook CameraFreeLook;
}
