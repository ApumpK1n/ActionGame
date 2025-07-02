
using UnityEngine;

namespace CombatAbilitySystem
{
    public abstract class ModifierMagnitudeBaseConfig : ScriptableObject
    {
        public abstract void Init(EffectExecutor executor);

        public abstract float CalculateMagnitude(EffectExecutor executor);
    }
}
