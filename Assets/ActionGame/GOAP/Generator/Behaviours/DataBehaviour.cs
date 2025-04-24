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

        public bool IsNear(Vector3 pos1, Vector3 pos2)
        {
            return Vector3.Distance(pos1, pos2) < 1f;
        }

        public bool IsNearAttackTarget()
        {
            if (AttackTarget == null) return true;

            float distance = Vector3.Distance(this.transform.position, AttackTarget.position);
            return distance < 0.5f;
        }

        public Vector3 SampleAttackTargetPosition()
        {
            if (AttackTarget != null)
            {
                return AttackTarget.transform.position;
            }
            return Vector3.zero;
        }
    }
}
