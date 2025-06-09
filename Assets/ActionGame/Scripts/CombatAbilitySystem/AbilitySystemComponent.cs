using System.Collections.Generic;
using UnityEngine;


/*
 思考：
1.需要实现一个完全配置化的技能系统
那么技能的整体流程：技能指令->技能流程：执行条件-》扣除消耗 -> 执行效果 播放动画 播放特效 音效
疑问: 怎么处理数值？ 答：Modifyer 嗯，这里需要实现一些通用的Modify 如加减乘除 还需要开放自定义的Modifyer
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
        private Dictionary<int, AbilityComponent> grantedAbilities = new Dictionary<int, AbilityComponent>();

        public bool IsActive = false;

        public GameObject MonoGameObject { get; private set; }

        public AbilitySystemComponent(GameObject go)
        {
            this.MonoGameObject = go;
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

        public bool TryApplyGameEffect(AbilityConfig abilityConfig)
        {

            return true;
        }
    }
}
