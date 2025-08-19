using System.Collections.Generic;
using UnityEngine;


/*
 思考：
1.需要实现一个完全配置化的技能系统
那么技能的整体流程：技能指令->技能流程：执行条件-》扣除消耗 -> 执行效果 播放动画 播放特效 音效
疑问: 怎么处理数值？ 答：Modifyer 嗯，这里需要实现一些通用的Modify 如加减乘除 还需要开放自定义的Modifyer

TODO: 属性 done
TODO: 属性集 done
TODO: 效果 基础done
TODO: 能力 基础done
TODO: ASC 基础done
TODO: 数值Modify done
TODO: 技能执行体 基础done DOTA2里 技能的A帐和魔晶效果是跟着技能走的，即在没有魔晶时放出去的技能执行体逻辑并不一样，那么就需要有一个动态添加技能效果的功能附加到技能执行体上
TODO: 效果执行体 表现为具体在游戏中显示的GameObject 同一个技能可能存在多个效果执行体 取决于效果
TODO: 播放动画 特效 音效
 */


/// <summary>
/// 通过执行行为去应用效果、修改数据来影响角色的属性或状态。
/// 能力加载具体的行为配置，单个行为配置确定触发时机、判断执行条件、执行具体效果等。
/// </summary>
///

namespace CombatAbilitySystem
{
    public class AbilitySystemComponent : ITick, ILateTick
    {
        private Dictionary<int, AbilityComponent> grantedAbilities;
        private AttributeSet attributeSet;

        public bool IsActive = false;
        public AttributeSet AttributeSet => attributeSet;

        public GameObject MonoGameObject { get; private set; }

        private List<EffectExecutor> effectExecutorDurationList;

        public AbilitySystemComponent(GameObject go, int attributeCapacity)
        {
            this.MonoGameObject = go;
            attributeSet = new AttributeSet(attributeCapacity);
            grantedAbilities = new Dictionary<int, AbilityComponent>();
            effectExecutorDurationList = new List<EffectExecutor>();
        }

        public bool IsValid()
        {
            return IsActive;
        }

        public void Tick(float deltaTime)
        {
            foreach (AbilityComponent ability in grantedAbilities.Values)
            {
                ability.Tick(deltaTime);
            }

            TickDurationEffects(deltaTime);
            TryRemoveDurationEffects();
        }

        public void LateTick(float deltaTime)
        {
            foreach (AbilityComponent ability in grantedAbilities.Values)
            {
                ability.LateTick(deltaTime);
            }
        }

        /// <summary>
        /// 添加能力
        /// </summary>
        /// <param name="abilityConfig"></param>
        /// <returns></returns>
        public T GrantAbility<T>(AbilityConfig abilityConfig) where T : AbilityComponent, new()
        {
            AbilityComponent ability = AbilityComponent.Create<T>(abilityConfig, this);
            grantedAbilities[abilityConfig.Id] = ability;

            return (T)ability;
        }

        public bool IsGrantedAbility(int abilityId)
        {
            return grantedAbilities.ContainsKey(abilityId);
        }

        /// <summary>
        /// 删除能力
        /// </summary>
        /// <param name="abilityId"></param>
        /// <returns></returns>
        public bool TryRemoveAbility(int abilityId)
        {
            if (grantedAbilities.ContainsKey(abilityId))
            {
                grantedAbilities.Remove(abilityId);
                return true;
            }
            return false;
        }

        public void InitAttributes(List<AttributeConfig> attributeConfigs)
        {
            foreach (AttributeConfig attributeConfig in attributeConfigs)
            {
                attributeSet.AddAttribute(attributeConfig);
            }

        }

        /// <summary>
        /// 施法
        /// </summary>
        /// <param name="abilityId"></param>
        /// <returns></returns>
        public bool TryActivateAbility(int abilityId)
        {
            if (!grantedAbilities.ContainsKey(abilityId)) return false;

            AbilityComponent abilityComponent = grantedAbilities[abilityId];
            AdvancedCoroutineManager.Instance.StartCoroutineEx(abilityComponent.TryActivate());
            return true;
        }

        public void TryApplyGameEffect(AbilityComponent abilityComponent, float level)
        {
            if (abilityComponent == null) return;

            foreach(var effectConfig in abilityComponent.Config.Effects)
            {
                // TODO：对象池
                EffectExecutor effectExecutor = EffectExecutor.Create(this, abilityComponent, effectConfig, level);

                TryApplyGameEffect(effectExecutor);
            }

           
        }


        /// <summary>
        /// 施加效果
        /// </summary>
        /// <param name="effectExecutor"></param>
        /// <returns></returns>
        public bool TryApplyGameEffect(EffectExecutor effectExecutor)
        {
            if (effectExecutor == null) return false;

            switch (effectExecutor.EffectConfig.DurationType)
            {
                case DurationType.HasDuration:
                case DurationType.Infinite:
                    ApplyDurationalGameplayEffect(effectExecutor);
                    break;
                case DurationType.Instant:
                    ApplyInstantGameplayEffect(effectExecutor);
                    return true;
            }

            return true;
        }

        void ApplyInstantGameplayEffect(EffectExecutor effectExecutor)
        {
            for (var i = 0; i < effectExecutor.EffectConfig.Modifiers.Length; i++)
            {
                var modifier = effectExecutor.EffectConfig.Modifiers[i];
                var magnitude = modifier.ModifierMagnitude.CalculateMagnitude(effectExecutor) * modifier.BaseValue;
                this.attributeSet.SetAttributeBaseValueModify(modifier, magnitude);
            }
        }

        void ApplyDurationalGameplayEffect(EffectExecutor effectExecutor)
        {
            effectExecutorDurationList.Add(effectExecutor);
        }

        void TickDurationEffects(float deltaTime)
        {
            for(int i=0; i<effectExecutorDurationList.Count; i++)
            {
                var effect = effectExecutorDurationList[i];
                effect.Tick(deltaTime);
                if (effect.CanPeriodTick)
                {
                    ApplyInstantGameplayEffect(effect);
                }
            }
        }

        void TryRemoveDurationEffects()
        {
            for (int i = 0; i < effectExecutorDurationList.Count; i++)
            {
                var effect = effectExecutorDurationList[i];
                if (effect.IsEnd)
                {
                    effectExecutorDurationList.RemoveAt(i);
                }
            }
        }
    }
}
