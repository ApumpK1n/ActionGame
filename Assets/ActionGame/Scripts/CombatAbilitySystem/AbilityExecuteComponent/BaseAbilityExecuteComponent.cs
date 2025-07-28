using UnityEngine;

namespace CombatAbilitySystem
{
    public abstract class BaseAbilityExecuteComponent : MonoBehaviour
    {
        public AbilitySystemComponent Owner;
        public AbilityComponent AbilityComponent;
    }
}
