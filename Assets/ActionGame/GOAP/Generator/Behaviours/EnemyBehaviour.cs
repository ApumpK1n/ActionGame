using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using CrashKonijn.Goap.Core;
using CrashKonijn.Agent.Core;


namespace CrashKonijn.Goap.ActionGame
{// 在一定区域内巡逻 遇到玩家 会进行攻击

    public class EnemyBehaviour : MonoBehaviour
    {
        private AgentBehaviour agent;
        private GoapActionProvider provider;
        private GoapBehaviour goap;

        private DataBehaviour data;

        #region Move
        private bool shouldMove;
        private ITarget currentTarget;

        private void OnEnable()
        {

        }

        private void OnDestroy()
        {
            this.agent.Events.OnTargetInRange -= this.OnTargetInRange;
            this.agent.Events.OnTargetChanged -= this.OnTargetChanged;
            this.agent.Events.OnTargetNotInRange -= this.TargetNotInRange;
            this.agent.Events.OnTargetLost -= this.TargetLost;
        }


        private void TargetLost()
        {
            this.currentTarget = null;
            this.shouldMove = false;
        }

        private void OnTargetInRange(ITarget target)
        {
            this.shouldMove = false;
        }

        private void OnTargetChanged(ITarget target, bool inRange)
        {
            this.currentTarget = target;
            this.shouldMove = !inRange;
        }

        private void TargetNotInRange(ITarget target)
        {
            this.shouldMove = true;
        }

        public void Tick(float deltaTime)
        {
            if (this.agent.IsPaused)
                return;

            if (!this.shouldMove)
                return;

            if (this.currentTarget == null)
                return;

            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.currentTarget.Position.x, this.transform.position.y, this.currentTarget.Position.z), deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (this.currentTarget == null)
                return;

            Gizmos.DrawLine(this.transform.position, this.currentTarget.Position);
        }
        #endregion

        public void Setup(GoapBehaviour goapBehaviour)
        {
            Debug.Log("Setup");
            this.goap = goapBehaviour;
            this.agent = this.GetComponent<AgentBehaviour>();
            this.provider = this.GetComponent<GoapActionProvider>();
            this.data = this.GetComponent<DataBehaviour>();

            if (this.provider.AgentTypeBehaviour == null)
                this.provider.AgentType = this.goap.GetAgentType(Define.ScriptEnemyAgent.ToString());

            this.agent.Events.OnTargetInRange += this.OnTargetInRange;
            this.agent.Events.OnTargetChanged += this.OnTargetChanged;
            this.agent.Events.OnTargetNotInRange += this.TargetNotInRange;
            this.agent.Events.OnTargetLost += this.TargetLost;
        }

        public void RequestGoal<TGoal>(bool resolve = true) where TGoal : IGoal
        {
            provider.RequestGoal(typeof(TGoal), resolve);
        }


    }


}
