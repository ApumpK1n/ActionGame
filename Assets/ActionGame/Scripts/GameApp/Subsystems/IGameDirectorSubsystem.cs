public interface IGameDirectorSubsystem : IGameSystem
{
    GameDirector GetGameDirector();

    void SetGameDirector(GameDirector gameDirector);
}
