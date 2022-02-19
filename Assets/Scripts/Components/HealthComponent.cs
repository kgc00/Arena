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
        public int MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDead;
        public bool Invulnerable { get; private set; }
        private StatsComponent _statsComponent;

        public HealthComponent Initialize(Unit owner, HealthData healthData, StatsComponent statsComponent) {
            Owner = owner;
            MaxHp = StatHelpers.GetMaxHealth(healthData.maxHp, statsComponent.Stats);
            CurrentHp = MaxHp;
            _statsComponent = statsComponent;

            Invulnerable = healthData.invulnerable;
            
            return this;
        }

        public void ReinitializeAbilities() {
            var healthPercentage = Math.Min(CurrentHp / MaxHp, 1);
            MaxHp = StatHelpers.GetMaxHealth(MaxHp, _statsComponent.Stats);
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
            
            // can be triggered multiple times if multiple damage sources proc while the unit is deleting
            if (CurrentHp <= 0 && !IsDead) {
                // Debug.Log($"{Owner} died");
                IsDead = true;
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