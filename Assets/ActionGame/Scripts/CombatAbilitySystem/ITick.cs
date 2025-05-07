namespace CombatAbilitySystem
{
    public interface ITick
    {
        public void Tick(float dt);
    }

    public interface ILateTick
    {
        public void LateTick(float dt);
    }
}
