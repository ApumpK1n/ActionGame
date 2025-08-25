# ActionGame
An action game made with the Unity engine

全局统一tick

# 角色
每个角色一个独立timescale方便控制

打击感：卡肉：改timescale 震屏：camera shake 

人物移动： 暂时Locomotion + Animancer + Rigidbody + IK 分层状态机

敌人行为：GOAP

# 武器
搓招： 输入系统->当前状态->最终招式


TODO:
1. 角色数据和逻辑分离，分基础属性以及当前属性
2. 招式配置化配置
3. 不直接更改角色数据，需要抽象出Modify 不同类型的Modify用不同类型的方式作用于角色属性（+-x）等等 解决属性更改来源 以及流程问题
4. 人物技能开发
5. 棍子武器派生招式完善
6. 小怪基础的 巡逻+休息+遇敌 AI调整

# 关于输入系统
使用Unity InputSystem做不同平台的键位处理，单个键位和键位槽绑定，键位事件触发后，触发的是玩家当前配置在键位槽的技能/其他 触发的行为，后期扩展键位槽即可。
TOOD: 1.支持改键，键位映射分为两套，默认一套 和自定义配置一套，同时支持导入导出。