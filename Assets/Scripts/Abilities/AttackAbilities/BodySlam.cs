using System.Collections;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class BodySlam : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            OnAbilityActivationFinished(Owner, this);
            ExecuteOnAbilityFinished();
            yield break;
        }

        public void OnCollisionEnter(Collision other) => AbilityConnected(other.gameObject, default);

        protected override void AbilityConnected(GameObject targetedUnit, GameObject _) {
            if (!targetedUnit.gameObject.TryGetComponent(out Unit objectAsUnit)) return;
            if (objectAsUnit.Owner.ControlType == ControlType.Ai) return;

            objectAsUnit.HealthComponent.DamageOwner(Damage, this, Owner);
            MonoHelper.SpawnVfx(VfxType.PlayerImpact, objectAsUnit.transform.position);
        }
    }
}