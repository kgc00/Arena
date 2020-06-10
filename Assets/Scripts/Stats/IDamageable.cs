namespace Stats
{
    public interface IDamageable
    { 
        HealthComponent HealthComponent { get; }
        void UnitDeath();
    }
}