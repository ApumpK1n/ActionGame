

using CombatAbilitySystem;
using UnityEngine;

/// <summary>
/// 发射弹体类的技能
/// </summary>
public class ProjectileAbilityComponent : AbilityComponent
{
    public override void CancelAbility()
    {
        
    }

    protected override void PreActivate()
    {

    }

    protected override void Activate()
    {
        ProjectileAbilityConfig config = (ProjectileAbilityConfig)Config;
        Projectile projectile = GameObject.Instantiate(config.ProjectilePrefab, this.Owner.MonoGameObject.transform.position, config.ProjectilePrefab.transform.rotation);
        projectile.AddForce(this.Owner.MonoGameObject.transform.forward);
    }

    protected override void OnEndAbility()
    {
        
    }
}
