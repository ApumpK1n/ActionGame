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
        public AbilitySystemComponent Owner { get; private set; }

        private float castPointTimer = 0f;
        public bool IsActive { get; private set; }

        public void Tick(float dt)
        {
            if (IsActive)
            {
                castPointTimer += dt;
            }
        }

        /// <summary>
        /// 尝试激活技能
        /// </summary>
        public IEnumerator TryActivate()
        {
            if (!CanActivateAbility())
            {
                EndAbility();
                yield break;
            }
            yield return ActivateAbility();
        }

        public bool CanActivateAbility()
        {
            return true;
        }

        private IEnumerator ActivateAbility()
        {
            IsActive = true;
            castPointTimer = 0f;
            yield return CastPoint(); // 抬手
            yield return PreActivate(); // 预激活
            yield return Activate(); // 激活
            EndAbility();
        }

        // 抬手
        private IEnumerator CastPoint()
        {
            if (Config.CastPoint <= 0) yield break;
            if (castPointTimer <= Config.CastPoint) yield return null;
        }

        /// <summary>
        /// 应用效果
        /// </summary>
        public void ApplyEffects()
        {

        }

        public void EndAbility()
        {
            IsActive = false;
            OnEndAbility();
        }

        protected abstract IEnumerator PreActivate();
        protected abstract IEnumerator Activate();

        protected abstract void OnEndAbility();

        public abstract void CancelAbility();

        public static T Create<T>(AbilityConfig config, AbilitySystemComponent owner) where T: AbilityComponent, new()
        {
            T ability = new T();
            ability.Config = config;
            ability.Owner = owner;
            return ability;
        }
    }
}
