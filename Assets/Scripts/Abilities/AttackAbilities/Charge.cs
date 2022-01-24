using System.Collections;
using Controls;
using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class Charge : MovementAttackAbility {
        private Vector3 startLocation = new Vector3(-999, -999, -999);
        private Vector3 targetLocation = new Vector3(-999, -999, -999);
        public bool ImpactedWall { get; private set; }
        // should parameterize this on the model/data
        public float WallImpactStunDuration { get; private set; } = 3f;
        private float distanceTraveled;
        private Rigidbody rb;
        private Vector3 heading;

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            if (rb == null) {
                rb = Owner.GetComponent<Rigidbody>();
            }
            startLocation = Owner.transform.position;
            this.targetLocation = targetLocation;
            heading = targetLocation - startLocation;
            heading.y = 0f;
            heading = heading.normalized;
            distanceTraveled = 0f;
            ImpactedWall = false;

            yield return new WaitForSeconds(StartupTime);

            OnAbilityActivationFinished(Owner, this);

            Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, MovementSpeedModifier);
            Owner.InputModifierComponent
                .AddModifier(InputModifier.CannotMove)
                .AddModifier(InputModifier.CannotRotate)
                .AddModifier(InputModifier.CannotAct);

            while (distanceTraveled < Range) {
                if (ImpactedWall) break;
                distanceTraveled = Vector3.Distance(startLocation, Owner.transform.position);
                yield return new WaitForFixedUpdate();
                var baseForce = Owner.StatsComponent.Stats.MovementSpeed.Value * heading;
                float deceleration = (1 - distanceTraveled / Range) + 0.2f;
                var force = distanceTraveled / Range < 0.8f
                    ? baseForce
                    : deceleration * baseForce;
                rb.AddForce(force);
            }

            Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, MovementSpeedModifier);
            Owner.InputModifierComponent
                .RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate)
                .RemoveModifier(InputModifier.CannotAct);

            ExecuteOnAbilityFinished();
        }

        // should look into some alternative...
        // i'd prefer to hook some event or something and do all my collision checking in one place
        // rather than have multiple abilities all checking on collision enter.
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Board")) {
                ImpactedWall = true;
                this.PostNotification(NotificationType.ChargeDidImpactWall);
            }
            if (other.gameObject.GetComponentInChildren<Unit>() != null) AbilityConnected(other.gameObject);
        }

        protected override void AbilityConnected(GameObject target, GameObject projectile = null) {
            // checking for null is done in the collision enter method
            var unit = target.GetComponent<Unit>();
            if (AffectedFactions.Contains(unit.Owner.ControlType)) unit.HealthComponent.DamageOwner(Damage, this, Owner);
        }
    }
}