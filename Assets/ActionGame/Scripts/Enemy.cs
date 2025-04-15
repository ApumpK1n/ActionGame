
using CrashKonijn.Goap.ActionGame;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

[RequireComponent(typeof(EnemyBehavior))]
[RequireComponent(typeof(AnimationComponent))]
public class Enemy : MonoBehaviour
{
    private EnemyBehavior behavior;

    private void Awake()
    {
        behavior = GetComponent<EnemyBehavior>();
    }

    private void Start()
    {
        behavior.RequestGoal<IdleGoal>();
    }

    public void Setup(GoapBehaviour goapBehaviour)
    {
        behavior.Setup(goapBehaviour);
    }

    public void Tick(float deltaTime)
    {
        behavior.Tick(deltaTime);
    }
}


public static class EnemyAnimationLayer
{
    public static int Base = 0;
}
