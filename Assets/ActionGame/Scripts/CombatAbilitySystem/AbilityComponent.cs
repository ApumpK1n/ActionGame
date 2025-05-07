using System;
using System.Collections;
using System.Collections.Generic;

namespace CombatAbilitySystem
{
    /// <summary>
    /// 单个技能的逻辑
    /// </summary>
    public abstract class AbilityComponent : ITick
    {
        public AbilityConfig Config { get; private set; }

        public void Tick(float dt)
        {

        }

        /// <summary>
        /// 尝试激活技能
        /// </summary>
        public void TryActivate()
        {
            if (!CanActivateAbility())
            {
                EndAbility();
                return;
            }

            ActivateAbility();
        }

        public bool CanActivateAbility()
        {
            return true;
        }

        private void ActivateAbility()
        {
            //TODO: 定时任务委托给能力执行体执行
            //TODO: 抬手时间
            PreActivate();
            Activate();
            ApplyEffects();
            EndAbility();
        }

        /// <summary>
        /// 应用效果
        /// </summary>
        private void ApplyEffects()
        {

        }

        public void EndAbility()
        {
            OnEndAbility();
        }

        protected abstract void PreActivate();
        protected abstract void Activate();

        protected abstract void OnEndAbility();

        public abstract void CancelAbility();

        public static T Create<T>(AbilityConfig config) where T: AbilityComponent, new()
        {
            T ability = new T();
            ability.Config = config;
            return ability;
        }
    }
}
