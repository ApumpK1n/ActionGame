public interface IGameApp
{
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize();

    /// <summary>
    /// 每帧运行
    /// </summary>
    /// <param name="dt">每帧时长</param>
    void Tick(float dt);

    /// <summary>
    /// 游戏结束
    /// </summary>
    void OnShutDown();

    void StartGame();

    /// <summary>
    /// 是否有切换后台的处理
    /// </summary>
    /// <param name="onFoucs">
    /// true - 显示
    /// false - 切换在后台
    /// </param>
    void OnFoucs(bool onFoucs);

    /// <summary>
    /// APP级的子系统（注：子系统是为了替换滥用的单例）
    /// </summary>
    /// <returns>
    /// GameApp级别范围的子系统
    /// </returns>
    IGameAppSubsystem GetSubsystemBase();

    /// <summary>
    /// Get a Subsystem of specified type
    /// </summary>
    /// <typeparam name="TSubSystem"></typeparam>
    /// <returns></returns>
    TSubSystem GetSubsystem<TSubSystem>() where TSubSystem : IGameAppSubsystem, new();
}
