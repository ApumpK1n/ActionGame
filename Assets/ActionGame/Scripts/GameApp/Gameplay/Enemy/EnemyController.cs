
using CombatAbilitySystem;
using UnityEngine;
using System.Collections.Generic;

public class EnemyController : ControllerBase
{
    [SerializeField] private List<AttributeConfig> Attributes;
    private AbilitySystemComponent abilitySystemComponent;

    void Awake()
    {
        //abilitySystemComponent = new AbilitySystemComponent(this.gameObject, 10);

        //abilitySystemComponent.InitAttributes(Attributes);
    }

    public void ApplyGameEffect(AbilityComponent abilityComponent)
    {
        abilitySystemComponent.TryApplyGameEffect(abilityComponent, 1);
    }
}
