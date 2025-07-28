

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
            GameObject areaDamageObject = GameObject.Instantiate(config.Prefab);
            areaDamageObject.transform.position = Owner.MonoGameObject.transform.position + Owner.MonoGameObject.transform.forward * 5;
            areaDamageObject.GetComponent<AreaDamageComponent>().Owner = Owner;
            areaDamageObject.GetComponent<AreaDamageComponent>().AbilityComponent = this;
            yield return null;
        }

        protected override void OnEndAbility()
        {

        }
    }

}
