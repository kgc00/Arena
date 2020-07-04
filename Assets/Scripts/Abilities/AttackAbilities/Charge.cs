using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Data;
using Controls;
using Stats;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace Abilities.AttackAbilities {
    public class Charge : MovementAttackAbility {
        private Vector3 targetLocation = new Vector3(-999,-999,-999);
        private bool charging = false;
        private bool impactedWall = false;
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            Debug.Log("starting");
            this.targetLocation = targetLocation;
            OnAbilityActivationFinished(Owner, this); 
            
            Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, 450);
            Owner.InputModifierComponent
                .AddModifier(InputModifier.CannotMove)
                .AddModifier(InputModifier.CannotRotate)
                .AddModifier(InputModifier.CannotAct);

            charging = true;
            impactedWall = false;
            var timeLeft = Duration;
            while (timeLeft > 0 
                    && Vector3.Distance(Owner.transform.position, targetLocation) > 1f
                    && !impactedWall) {
                timeLeft = Clamp(timeLeft - Time.deltaTime, 0, Duration);
                yield return null;
            }
            charging = false;
            
            Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, 450);            

            // set a "stun" state for 1 second if we hit a wall during the charge
            if(impactedWall) yield return new WaitForSeconds(1);
            
            Owner.InputModifierComponent
                .RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate)
                .RemoveModifier(InputModifier.CannotAct);
            
            Debug.Log("ending");
            OnAbilityFinished(Owner, this);
        }

        private void FixedUpdate() {
            if (!charging) return;
            
            Vector3 heading = targetLocation - Owner.transform.position;
            heading.y = 0f;
            heading = Owner.StatsComponent.Stats.MovementSpeed.Value * heading.normalized;
            Owner.GetComponent<Rigidbody>().AddForce(heading);
        }

        protected override void AbilityConnected(GameObject targetedUnit, GameObject projectile = null) {
            Debug.Log("hit em");
        }

        private void OnDrawGizmos() {
            if (!charging) return;
            
            Gizmos.DrawSphere(targetLocation, 1);
        }

        private void OnCollisionEnter(Collision other) {
            if (charging && other.gameObject.CompareTag("Board")) impactedWall = true;
        }
    }
}