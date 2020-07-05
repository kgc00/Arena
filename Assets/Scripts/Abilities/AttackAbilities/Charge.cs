using System.Collections;
using Controls;
using Stats;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace Abilities.AttackAbilities {
    public class Charge : MovementAttackAbility {
        private Vector3 targetLocation = new Vector3(-999,-999,-999);
        private bool charging;
        public bool ImpactedWall { get; private set; }
        // should parameterize this on the model/data
        public float WallImpactStunDuration { get; private set; } = 3f;

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            Debug.Log("starting");
            this.targetLocation = targetLocation;
            OnAbilityActivationFinished(Owner, this); 
            
            Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, MovementSpeedModifier);
            Owner.InputModifierComponent
                .AddModifier(InputModifier.CannotMove)
                .AddModifier(InputModifier.CannotRotate)
                .AddModifier(InputModifier.CannotAct);

            charging = true;
            ImpactedWall = false;
            var timeLeft = Duration;
            while (timeLeft > 0 
                    && Vector3.Distance(Owner.transform.position, targetLocation) > 1f
                    && !ImpactedWall) {
                timeLeft = Clamp(timeLeft - Time.deltaTime, 0, Duration);
                yield return null;
            }
            charging = false;
            
            Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, MovementSpeedModifier);
            
            Owner.InputModifierComponent
                .RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate)
                .RemoveModifier(InputModifier.CannotAct);
            
            OnAbilityFinished(Owner, this);
        }

        private void FixedUpdate() {
            if (!charging) return;
            
            Vector3 heading = targetLocation - Owner.transform.position;
            heading.y = 0f;
            heading = Owner.StatsComponent.Stats.MovementSpeed.Value * heading.normalized;
            Owner.GetComponent<Rigidbody>().AddForce(heading);
        }

        private void OnDrawGizmos() {
            if (!charging) return;
            
            Gizmos.DrawSphere(targetLocation, 1);
        }

        // should look into some alternative...
        // i'd prefer to hook some event or something and do all my collision checking in one place
        // rather than have multiple abilities all checking on collision enter.
        private void OnCollisionEnter(Collision other) {
            if (charging && other.gameObject.CompareTag("Board")) ImpactedWall = true;
            if (charging && other.gameObject.GetComponentInChildren<Unit>() != null) AbilityConnected(other.gameObject);
        }

        protected override void AbilityConnected(GameObject target, GameObject projectile = null) {
            // checking for null is done in the collision enter method
            var unit = target.GetComponent<Unit>();
            if(AffectedFactions.Contains(unit.Owner.ControlType)) unit.HealthComponent.DamageOwner(Damage, this, Owner);
        }
    }
}