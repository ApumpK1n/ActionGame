using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    public class DataBehaviour : MonoBehaviour
    {
        public float Fatigue = 0;

        public bool IsIdle;

        public bool IsWander;

        public Transform BelongArea;

        public Transform AttackTarget;

        public EnemyConfig EnemyConfig { get; set; }

        public bool IsNearAttackTarget()
        {
            if (AttackTarget == null) return true;

            float distance = Vector3.Distance(this.transform.position, AttackTarget.position);
            return distance < 0.5f;
        }
    }
}
