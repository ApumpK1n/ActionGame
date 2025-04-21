
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

    private void Awake()
    {
        behaviour = GetComponent<EnemyBehaviour>();
        dataBehaviour = GetComponent<DataBehaviour>();
    }

    private void Start()
    {
        behaviour.RequestGoal<WanderGoal>();
    }

    public void Setup(GoapBehaviour goapBehaviour, Transform belongArea)
    {
        behaviour.Setup(goapBehaviour);
        dataBehaviour.BelongArea = belongArea;
    }

    public void Tick(float deltaTime)
    {
        behaviour.Tick(deltaTime);
    }
}


public static class EnemyAnimationLayer
{
    public static int Base = 0;
}
