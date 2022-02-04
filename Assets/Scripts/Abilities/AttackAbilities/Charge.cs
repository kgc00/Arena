using System.Collections;
using Common;
using Controls;
using Data;
using Data.Types;
using Projectiles;
using State;
using Units;
using UnityEngine;
using Utils;
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
        private bool _active;

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
            this.PostNotification(NotificationType.AbilityWillActivate, new UnitIntent(this, new TargetingData(TargetingBehavior.TargetLocation, targetLocation), Owner));

            yield return new WaitForSeconds(StartupTime);
            _active = true;
            this.PostNotification(NotificationType.AbilityDidActivate, new UnitIntent(this, new TargetingData(TargetingBehavior.TargetLocation, targetLocation), Owner));

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

            _active = false;
            ExecuteOnAbilityFinished();
        }

        // should look into some alternative...
        // i'd prefer to hook some event or something and do all my collision checking in one place
        // rather than have multiple abilities all checking on collision enter.
        private void OnCollisionEnter(Collision other) {
            if (!_active) return;
            if (other.gameObject.CompareTag("Board")) {
                ImpactedWall = true;
                this.PostNotification(NotificationType.ChargeDidImpactWall);
            }

            var target = other.gameObject.GetComponentInChildren<Unit>();
            if (target == null) return;
            _other = other;
            AbilityConnected(target.gameObject);
        }
        private Collision _other;
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) {
            // checking for null is done in the collision enter method
            var unit = target.gameObject.GetComponent<Unit>();
            if (AffectedFactions.Contains(unit.Owner.ControlType)) {
                unit.HealthComponent.DamageOwner(Damage, this, Owner);
                MonoHelper.SpawnVfx(VfxType.PlayerImpact, _other.contacts[0].point);
                if (unit.TryGetComponent<Rigidbody>(out var rb)) {
                    // todo - lower force, extend duration
                    rb.AddForce(_other.contacts[0].normal * Force, ForceMode.Impulse);
                }
            }
        }
    }
}