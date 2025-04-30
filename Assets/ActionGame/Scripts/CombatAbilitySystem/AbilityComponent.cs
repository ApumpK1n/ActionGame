using System;
using System.Collections;
using System.Collections.Generic;

namespace CombatAbilitySystem
{
    /// <summary>
    /// 单个技能的逻辑
    /// </summary>
    public class AbilityComponent
    {
        public AbilityConfig Config;

        public void TryActivate()
        {

        }

        public static T Create<T>(AbilityConfig config) where T: AbilityComponent, new()
        {
            T ability = new T();
            ability.Config = config;
            return ability;
        }
    }
}
