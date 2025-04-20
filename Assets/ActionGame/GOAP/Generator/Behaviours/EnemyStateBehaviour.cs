using UnityEngine;

namespace CrashKonijn.Goap.ActionGame
{
    public class EnemyStateBehaviour : MonoBehaviour
    {
        public EnemyState State;
    }

    public enum EnemyState
    {
        None,
        Idle,
        Wander,

    }
}
