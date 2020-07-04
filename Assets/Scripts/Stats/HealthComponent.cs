using System;
using Stats.Data;
using Units;
using UnityEngine;

namespace Stats
{
    public class HealthComponent : MonoBehaviour
    {
        public static Action<Unit, float> OnHealthChanged = delegate { };
        public Unit Owner { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        public bool Invulnerable { get; private set; }  = false;

        public HealthComponent Initialize (Unit owner, HealthData healthData) {
            this.Owner = owner;
            this.MaxHp = healthData.maxHp;
            CurrentHp = MaxHp;

            Invulnerable = healthData.invulnerable;

            // Debug.Log($"Spawning: {Owner.name} with a max HP of {MaxHp}");
            return this;
        }
        

        public void TakeDamage (float amount) {
            // Debug.Log($"Adjusting {Owner.name}'s current health by {amount}.");
            
            if (Invulnerable) return;
            AdjustHealth(-Math.Abs(amount));
        }

        private void AdjustHealth(float amount) {
            var prevAmount = CurrentHp;
            var newAmount = Mathf.Clamp(CurrentHp + amount, 0, MaxHp);

            // Debug.Log($"Adjusting {Owner.name}'s current health from {prevAmount} to {newAmount}.");

            CurrentHp = newAmount;

            OnHealthChanged(Owner, prevAmount);
            
            if (CurrentHp <= 0) {
                Debug.Log($"{Owner} died");
                Owner.UnitDeath();
                
            }
        }

        internal void Refill () {
            var prevAmount = CurrentHp;
            CurrentHp = MaxHp;

            OnHealthChanged (Owner, prevAmount);
        }
    }
}