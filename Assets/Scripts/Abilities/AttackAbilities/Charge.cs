using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Data;
using Stats;
using Units;
using UnityEngine;

namespace Abilities.AttackAbilities {
    public class Charge : AttackAbility {
        private Vector3 debugTarget;
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            debugTarget = targetLocation;
            Debug.Log("starting");
            onAbilityActivationFinished(Owner, this);

            Owner.statsComponent.IncrementStat(StatType.MovementSpeed, 100);
            yield return new WaitUntil(() => Vector3.Distance(Owner.transform.position, targetLocation) < 1f);
            Owner.statsComponent.DecrementStat(StatType.MovementSpeed, 100);            
            Debug.Log("ending");

            onAbilityFinished(Owner, this);
        }

        protected override void AbilityConnected(GameObject targetedUnit, GameObject projectile = null) {
            Debug.Log("hit em");
        }

        private void OnDrawGizmos() {
            Gizmos.DrawSphere(debugTarget, 1);
        }
    }
}