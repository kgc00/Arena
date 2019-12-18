using Units;
using UnityEngine;

namespace Stats
{
    public class HealthComponent : MonoBehaviour
    {
        public static System.Action<Unit, float> onHealthChanged = delegate { };
        public Unit Owner;
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        
        public void Initialize (Unit owner) {
            this.Owner = owner;
            this.MaxHp = owner.BaseStats.Health.Value;
            CurrentHp = MaxHp;
        }

        public void AdjustHealth (float amount) {
            var prevAmount = CurrentHp;
            CurrentHp = Mathf.Clamp (CurrentHp + amount, 0, MaxHp);

            if (CurrentHp <= 0)
            {
                Debug.Log("we died");
                // unit has died
                // Owner.UnitDeath();
            }

            onHealthChanged (Owner, prevAmount);
        }

        internal void Refill () {
            var prevAmount = CurrentHp;
            CurrentHp = MaxHp;

            onHealthChanged (Owner, prevAmount);
        }
    }
}