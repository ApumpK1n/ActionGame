
using Cinemachine;
using CrashKonijn.Goap.Runtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大世界场景管理类
/// </summary>

public class WorldScene : MonoBehaviour
{
    public Player Player;

    [SerializeField] public Player PlayerPrefab;
    [SerializeField] public Transform PlayerReborn;
    [SerializeField] public Weapon StickWeaponPrefab;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineFreeLook playerFollowCamera;

    [SerializeField] private Animator debugPlayerCombatAnimator;
    [SerializeField] private Animator debugPlayerMovementAnimator;

    #region Enemy
    [SerializeField] private GoapBehaviour goapBehaviour;
    [SerializeField] private List<Enemy> enemies;
    #endregion

    #region Scene
    [SerializeField, Header("大世界区域")] private List<Transform> Areas;
    #endregion


    public void Setup()
    {
        Player = Instantiate(PlayerPrefab, PlayerReborn, false);
        Player.Setup(this);

        playerFollowCamera.Follow = Player.transform;
        playerFollowCamera.LookAt = Player.Neck;

        Player.AddWeapon(StickWeaponPrefab);

        Player.DebugCombatAnimator = debugPlayerCombatAnimator;
        Player.DebugMovementAnimator = debugPlayerMovementAnimator;

        foreach (Enemy enemy in enemies)
        {
            enemy.Setup(goapBehaviour, Areas[0], this);
        }
    }

    public void Tick(float deltaTime)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Tick(deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward);

    }

    public Transform GetPlayerCamera()
    {
        return playerCamera.transform;
    }
}
