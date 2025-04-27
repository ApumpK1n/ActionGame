
using CrashKonijn.Goap.ActionGame;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviour))]
[RequireComponent(typeof(DataBehaviour))]
[RequireComponent(typeof(AnimationComponent))]
public class Enemy : MonoBehaviour
{
    private EnemyBehaviour behaviour;
    private DataBehaviour dataBehaviour;

    [SerializeField] private EnemyConfig config;
    private WorldScene belongWorldScene;
    private void Awake()
    {
        behaviour = GetComponent<EnemyBehaviour>();
        dataBehaviour = GetComponent<DataBehaviour>();
    }

    private void Start()
    {
        behaviour.GoapActionProvider.RequestGoal<WanderGoal, IdleGoal, GuardGoal>();
    }

    public void Setup(GoapBehaviour goapBehaviour, Transform belongArea, WorldScene worldScene)
    {
        behaviour.Setup(goapBehaviour);
        dataBehaviour.BelongArea = belongArea;
        dataBehaviour.EnemyConfig = config;
        this.belongWorldScene = worldScene;
    }

    public void Tick(float deltaTime)
    {
        behaviour.Tick(deltaTime);

        float distance = Vector3.Distance(transform.position, belongWorldScene.Player.transform.position);
        if (distance <= 50f)
        {
            dataBehaviour.AttackTarget = belongWorldScene.Player.transform;
        }
        else
        {
            dataBehaviour.AttackTarget = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);
    }
}


public static class EnemyAnimationLayer
{
    public static int Base = 0;
}
