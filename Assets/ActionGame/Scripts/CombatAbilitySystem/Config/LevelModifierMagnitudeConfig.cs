using UnityEngine;

namespace CombatAbilitySystem
{
    [CreateAssetMenu(menuName = "CombatAbilitySystem/ModifierMagnitude/LevelModifierMagnitudeConfig")]
    public class LevelModifierMagnitudeConfig : ModifierMagnitudeBaseConfig
    {
        [SerializeField] private AnimationCurve AnimationCurve;
        public override void Init(EffectExecutor executor)
        {

        }

        public override float CalculateMagnitude(EffectExecutor executor)
        {
            return AnimationCurve.Evaluate(executor.Level);
        }
    }
}
