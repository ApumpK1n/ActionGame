

using UnityEngine;
using System.Collections;

namespace CombatAbilitySystem
{
    /// <summary>
    /// AOE类的技能
    /// </summary>
    public class AreaDamageAbilityComponent : AbilityComponent
    {
        public override void CancelAbility()
        {

        }


        protected override IEnumerator Activate()
        {
            AreaDamageAbilityConfig config = (AreaDamageAbilityConfig)Config;

            yield return null;
        }

        protected override void OnEndAbility()
        {

        }
    }

}
