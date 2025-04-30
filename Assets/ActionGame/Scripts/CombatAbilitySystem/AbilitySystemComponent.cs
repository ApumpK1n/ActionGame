using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 通过执行行为去应用效果、修改数据来影响角色的属性或状态。
/// 能力加载具体的行为配置，单个行为配置确定触发时机、判断执行条件、执行具体效果等。
/// </summary>
///
/*
 思考：
1.需要实现一个完全配置化的技能系统
那么技能的整体流程：技能指令->技能流程：执行条件-》扣除消耗 -> 执行效果 播放动画 播放特效 音效
疑问: 怎么处理数值？ 答：Modifyer 嗯，这里需要实现一些通用的Modify 如加减乘除 还需要开放自定义的Modifyer
 */

namespace CombatAbilitySystem
{
    public class AbilitySystemComponent
    {
        private Dictionary<int, AbilityComponent> grantedAbilities = new Dictionary<int, AbilityComponent>();

        public bool IsActive = false;


        public bool IsValid()
        {
            return IsActive;
        }

        public void Tick(float deltaTime)
        {

        }


        public void GrantAbility(AbilityConfig abilityConfig)
        {

            grantedAbilities[abilityConfig.Id] = AbilityComponent.Create<AbilityComponent>(abilityConfig);
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
            abilityComponent.TryActivate();
            return true;
        }
    }
}
