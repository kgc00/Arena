using System.Collections;
using System.Collections.Generic;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class OrcSlash : AttackAbility {
        private bool isActive;
        private List<Unit> impactedUnits;
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            impactedUnits ??= new List<Unit>();
            yield return new WaitForSeconds(StartupTime); // the clip has some pull back before the swing, only enable the hitbox once the swing is in motion
            isActive = true;
            OnAbilityActivationFinished(Owner, this);
            yield return new WaitForSeconds(Duration - StartupTime); // subtract the startup time so we match the clip's length
            isActive = false;
            impactedUnits.Clear();
            ExecuteOnAbilityFinished();
        }

        public void OnCollisionEnter(Collision other) => AbilityConnected(other.gameObject, default);
        public void OnTriggerEnter(Collider other) => AbilityConnected(other.gameObject, default);

        protected override void AbilityConnected(GameObject targetedUnit, GameObject _) {
            if (!isActive) return;

            if (!targetedUnit.gameObject.TryGetComponent(out Unit objectAsUnit)) return;
            if (objectAsUnit.Owner.ControlType == ControlType.Ai) return;

            if (impactedUnits.Contains(objectAsUnit)) return; // this triggers on the same unit multiple times per hit, only allow the first to deal damage
            objectAsUnit.HealthComponent.DamageOwner(Damage, this, Owner);
            impactedUnits.Add(objectAsUnit);
            MonoHelper.SpawnVfx(VfxType.PlayerImpact, objectAsUnit.transform.position);
        }
    }
}