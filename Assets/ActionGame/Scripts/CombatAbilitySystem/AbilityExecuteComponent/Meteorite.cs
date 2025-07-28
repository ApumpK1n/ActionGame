using System.Collections;
using System.Collections.Generic;
using CombatAbilitySystem;
using UnityEngine;

namespace CombatAbilitySystem
{
    public class Meteorite : AreaDamageComponent
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == Layers.EnemyNumber)
            {
                other.gameObject.GetComponent<Enemy>().ApplyGameEffect(AbilityComponent);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == Layers.EnemyNumber)
            {
                collision.gameObject.GetComponent<Enemy>().ApplyGameEffect(AbilityComponent);
            }
        }
    }
}


