
using CrashKonijn.Goap.ActionGame;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

[RequireComponent(typeof(EnemyBehavior))]
[RequireComponent(typeof(AnimationComponent))]
public class Enemy : MonoBehaviour
{
    private EnemyBehavior behavior;
    private AnimationComponent animationComponent;

    private void Awake()
    {
        behavior = GetComponent<EnemyBehavior>();
        animationComponent = GetComponent<AnimationComponent>();
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
