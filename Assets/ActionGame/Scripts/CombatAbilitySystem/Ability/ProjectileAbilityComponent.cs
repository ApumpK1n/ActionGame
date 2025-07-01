

using CombatAbilitySystem;
using UnityEngine;
using System.Collections;

namespace CombatAbilitySystem
{

    /// <summary>
    /// 发射弹体类的技能
    /// </summary>
    public class ProjectileAbilityComponent : AbilityComponent
    {
        public override void CancelAbility()
        {

        }


        protected override IEnumerator Activate()
        {
            ProjectileAbilityConfig config = (ProjectileAbilityConfig)Config;
            Projectile projectile = GameObject.Instantiate(config.ProjectilePrefab, this.Owner.MonoGameObject.transform.position, config.ProjectilePrefab.transform.rotation);
            projectile.AddForce(this.Owner.MonoGameObject.transform.forward);
            yield return null;
        }

        protected override void OnEndAbility()
        {

        }
    }

}
