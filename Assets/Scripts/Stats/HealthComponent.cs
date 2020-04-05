using Stats.Data;
using Units;
using UnityEngine;

namespace Stats
{
    public class HealthComponent : MonoBehaviour
    {
        public static System.Action<Unit, float> OnHealthChanged = delegate { };
        public Unit Owner { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        
        public HealthComponent Initialize (Unit owner, HealthData healthData) {
            this.Owner = owner;
            this.MaxHp = healthData.maxHp;
            CurrentHp = healthData.currentHp;
            return this;
        }

        public void AdjustHealth (float amount) {
            // Debug.Log("adjusting health");
            var prevAmount = CurrentHp;
            CurrentHp = Mathf.Clamp (CurrentHp + amount, 0, MaxHp);

            // Debug.Log($"current health {CurrentHp}");
            if (CurrentHp <= 0)
            {
                Debug.Log($"{Owner} died");
                // unit has died
                Owner.UnitDeath();
            }

            OnHealthChanged (Owner, prevAmount);
        }

        internal void Refill () {
            var prevAmount = CurrentHp;
            CurrentHp = MaxHp;

            OnHealthChanged (Owner, prevAmount);
        }
    }
}