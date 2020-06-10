using System.Collections;
using System.Collections.Generic;
using Abilities.Data;
using Units;
using UnityEngine;

namespace Abilities.AttackAbilities {
    public class BodySlam : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            onAbilityActivationFinished(Owner, this);
            yield break;
        }

        protected override void AbilityConnected(GameObject targetedUnit, GameObject projectile) { }
    }
}