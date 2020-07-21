using System;
using System.Collections;
using Components;
using Status;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace Abilities.Buffs {
    public class MagicShield : BuffAbility {
        private float damageRemembered;
        private bool shielding;
        private Unit aggressor;
        private void OnEnable() => HealthComponent.OnDamageStarted += RememberPain;
        private void OnDisable() => HealthComponent.OnDamageStarted -= RememberPain;

        float InitializeState() {
            shielding = true;
            aggressor = null;
            return Duration;
        }
        void ResetState() {
            damageRemembered = 0;
            shielding = false;
        }

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            OnAbilityActivationFinished(Owner, this);
            
            var timeLeft = InitializeState();
            Owner.HealthComponent.SetInvulnerable();

            while (timeLeft > 0 && damageRemembered < 3) {
                timeLeft = Clamp(timeLeft - Time.deltaTime, 0, Duration);
                yield return null;
            }

            if (aggressor != null) {
                if (damageRemembered > 3) aggressor.gameObject.AddComponent<Slowed>().Initialize(aggressor, 7, 20);
                if (damageRemembered > 0) aggressor.gameObject.AddComponent<Slowed>().Initialize(aggressor, 3, 20);
            }

            // will need to hook up damage modifier from stats
            Owner.gameObject.AddComponent<DragonFury>().Initialize(Owner, 5, damageRemembered);

            ResetState();
            Owner.HealthComponent.SetVulnerable();
            OnAbilityFinished(Owner, this);
        }

        void RememberPain(Unit unit, Unit damageDealer, float amount) {
            if (!shielding) return;
            if (unit != Owner) return;

            damageRemembered += amount;
            aggressor = damageDealer;
        }
    }
}