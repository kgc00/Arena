using System;
using Abilities;
using Data.StatData;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        public static Action<Unit, float> OnHealthChanged = delegate { };
        public static Action<Unit, Unit, float> OnDamageStarted = delegate { };
        public Unit Owner { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        public bool Invulnerable { get; private set; }

        public HealthComponent Initialize (Unit owner, HealthData healthData) {
            Owner = owner;
            MaxHp = healthData.maxHp;
            CurrentHp = MaxHp;

            Invulnerable = healthData.invulnerable;

            Debug.Log($"Spawning: {Owner.name} with a max HP of {MaxHp}");
            return this;
        }

        public void UpdateModel(HealthData data) {
            var healthPercentage = Math.Min(CurrentHp / MaxHp, 1);
            MaxHp = data.maxHp;
            CurrentHp = MaxHp * healthPercentage;
        }

        public void SetInvulnerable() => Invulnerable = true;
        public void SetVulnerable() => Invulnerable = false;
        public void DamageOwner(float amount) {
            // Debug.Log($"Adjusting {Owner.name}'s current health by {amount}.");

            OnDamageStarted(Owner, null, amount);
            if (Invulnerable) return;
            AdjustHealth(-Math.Abs(amount));
        }

        public void DamageOwner(float amount, Ability damageSource, Unit damageDealer) {
            // Debug.Log($"Adjusting {Owner.name}'s current health by {amount}.");

            OnDamageStarted(Owner, damageDealer, amount);
            if (Invulnerable) return;
            AdjustHealth(-Math.Abs(amount));
        }

        public void HealOwner (float amount) {
            // Debug.Log($"Adjusting {Owner.name}'s current health by {amount}.");

            AdjustHealth(Math.Abs(amount));
        }
        
        /// <summary>
        /// Adjust unit's health:
        /// negative values => damage
        /// positive values => heal
        /// </summary>
        /// <param name="amount"></param>
        private void AdjustHealth(float amount) {
            var prevAmount = CurrentHp;
            var newAmount = Mathf.Clamp(CurrentHp + amount, 0, MaxHp);

            // Debug.Log($"Adjusting {Owner.name}'s current health from {prevAmount} to {newAmount}.");

            CurrentHp = newAmount;

            OnHealthChanged(Owner, prevAmount);
            
            if (CurrentHp <= 0) {
                // Debug.Log($"{Owner} died");
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