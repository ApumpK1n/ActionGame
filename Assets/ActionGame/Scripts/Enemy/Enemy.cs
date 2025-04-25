
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
    private void Awake()
    {
        behaviour = GetComponent<EnemyBehaviour>();
        dataBehaviour = GetComponent<DataBehaviour>();
    }

    private void Start()
    {
        behaviour.GoapActionProvider.RequestGoal<WanderGoal, IdleGoal, GuardGoal>();
    }

    public void Setup(GoapBehaviour goapBehaviour, Transform belongArea)
    {
        behaviour.Setup(goapBehaviour);
        dataBehaviour.BelongArea = belongArea;
        dataBehaviour.EnemyConfig = config;
    }

    public void Tick(float deltaTime)
    {
        behaviour.Tick(deltaTime);

        float distance = Vector3.Distance(transform.position, Game.Instance.Player.transform.position);
        if (distance <= 50f)
        {
            dataBehaviour.AttackTarget = Game.Instance.Player.transform;
        }
        else
        {
            dataBehaviour.AttackTarget = null;
        }
    }
}


public static class EnemyAnimationLayer
{
    public static int Base = 0;
}
