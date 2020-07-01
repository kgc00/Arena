using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Data;
using Controls;
using Stats;
using Units;
using UnityEngine;

namespace Abilities.AttackAbilities {
    public class Charge : AttackAbility {
        private Vector3 targetLocation = new Vector3(-999,-999,-999);
        private bool charging = false;
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            Debug.Log("starting");
            this.targetLocation = targetLocation;
            onAbilityActivationFinished(Owner, this);
            Owner.statsComponent.IncrementStat(StatType.MovementSpeed, 450);
            Owner.inputModifierComponent
                .AddModifier(InputModifier.CannotMove)
                .AddModifier(InputModifier.CannotRotate)
                .AddModifier(InputModifier.CannotAct);

            charging = true;
            yield return new WaitUntil(() => Vector3.Distance(Owner.transform.position, targetLocation) < 1f);
            charging = false;
            
            Owner.statsComponent.DecrementStat(StatType.MovementSpeed, 450);            

            Owner.inputModifierComponent
                .RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate)
                .RemoveModifier(InputModifier.CannotAct);
            
            Debug.Log("ending");
            onAbilityFinished(Owner, this);
        }

        private void FixedUpdate() {
            if (!charging) return;
            
            Vector3 heading = targetLocation - Owner.transform.position;
            heading.y = 0f;
            heading = Owner.statsComponent.Stats.MovementSpeed.Value * heading.normalized;
            Debug.Log("MOVEMENT SPEED: " + Owner.statsComponent.Stats.MovementSpeed.Value);
            Owner.GetComponent<Rigidbody>().AddForce(heading);
        }

        protected override void AbilityConnected(GameObject targetedUnit, GameObject projectile = null) {
            Debug.Log("hit em");
        }

        private void OnDrawGizmos() {
            if (!charging) return;
            
            Gizmos.DrawSphere(targetLocation, 1);
        }
    }
}