using CrashKonijn.Agent.Core;
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

        public Vector3 GoapAttackTargetPosition;

        public bool IsNearAttackTarget()
        {
            if (AttackTarget == null) return true;

            float distance = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(AttackTarget.position.x, AttackTarget.position.z));
            return distance < 2f;
        }

        public bool IsNear(Vector3 target)
        {
            float distance = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(target.x, target.z));
            return distance < 2f;
        }

        public bool IsNear()
        {
            return IsNear(GoapAttackTargetPosition);
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
