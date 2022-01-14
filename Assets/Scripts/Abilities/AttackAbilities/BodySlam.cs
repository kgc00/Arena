using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Abilities.AttackAbilities {
    public class BodySlam : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            OnAbilityActivationFinished(Owner, this);
            ExecuteOnAbilityFinished();
            yield break;
        }

        protected override void AbilityConnected(GameObject targetedUnit, GameObject projectile) { }
    }
}