namespace Components
{
    public interface IDamageable
    { 
        HealthComponent HealthComponent { get; }
        void UnitDeath();
    }
}