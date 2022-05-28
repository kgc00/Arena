using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Components;
using Data;
using Data.Params;
using Data.Types;
using Sirenix.Utilities;
using State;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;
using Debug = System.Diagnostics.Debug;
using Random = System.Random;

namespace Abilities.AttackAbilities {
    public class Disrupt : AttackAbility {
        private readonly string _disruptAoeEffectName = "Disrupt AoE Effect";

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            var targetRb = Locator.GetClosestVisiblePlayerUnit(targetLocation)?.GetComponent<Rigidbody>();
            if (targetRb == null) yield break;
            yield return new WaitForFixedUpdate();
            var updatedTargetLocation = targetRb.transform.position + targetRb.velocity * 50 * Time.fixedDeltaTime;
            UpdateLocationIfOccupied(ref updatedTargetLocation);
            CreateDisruptPlaceholderAoEComponent(updatedTargetLocation);
            this.PostNotification(NotificationType.AbilityWillActivate,
                new UnitIntent(this, new TargetingData(TargetingBehavior.TargetLocation, updatedTargetLocation),
                    Owner));
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.AbilityDidActivate,
                new UnitIntent(this, new TargetingData(TargetingBehavior.TargetLocation, updatedTargetLocation),
                    Owner));
            this.PostNotification(NotificationType.DidCastDisrupt);
            OnAbilityActivationFinished(Owner, this);
            SpawnAoEEffect(updatedTargetLocation);
            ExecuteOnAbilityFinished();
        }

        private void UpdateLocationIfOccupied(ref Vector3 updatedTargetLocation, int depth = 0) {
            // recusively find a new nearby location in a random direction if target is already occupied
            if (depth > 5) return;
            var overlappedColliders = Physics.OverlapSphere(updatedTargetLocation, AreaOfEffectCircularRadius,
                LayerMask.GetMask("Abilities"));
            foreach (var hitCollider in overlappedColliders) {
                if (hitCollider.gameObject.name != _disruptAoeEffectName) continue;
                updatedTargetLocation += new Vector3(UnityEngine.Random.Range(-1, 1f) * AreaOfEffectCircularRadius, 0, UnityEngine.Random.Range(-1, 1f) * AreaOfEffectCircularRadius);
                UpdateLocationIfOccupied(ref updatedTargetLocation, depth++);
            }
        }

        private void CreateDisruptPlaceholderAoEComponent(Vector3 targetLocation) {
            // use a 'placeholder' to say a disrupt will be spawned in this location shortly
            // avoids the scenario where many disrupts are placed by different units at the same time/location
            // does not interact with anything and does no damage
            var colliderParams = new SphereParams(AreaOfEffectCircularRadius);
            var aoeGo = new GameObject(_disruptAoeEffectName)
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    targetLocation,
                    targetLocation,
                    null,
                    null,
                    null,
                    AffectedFactions,
                    Force,
                    StartupTime
                )
                .gameObject;
            aoeGo.layer = LayerMask.NameToLayer("Abilities");
        }

        private void SpawnAoEEffect(Vector3 targetLocation) {
            var colliderParams = new SphereParams(AreaOfEffectCircularRadius);
            var aoeGo = new GameObject("Disrupt AoE Effect")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    targetLocation,
                    targetLocation,
                    HandleEnterStrategy,
                    null,
                    null,
                    AffectedFactions,
                    force: Force,
                    duration: Duration
                )
                .gameObject;
            aoeGo.layer = LayerMask.NameToLayer("Abilities");
            var vfx = MonoHelper.SpawnVfx(VfxType.DisruptStartup, targetLocation);
            vfx.AddComponent<SetParticleData>().Initialize(Duration, AreaOfEffectCircularRadius);
            vfx.transform.SetParent(aoeGo.transform);
        }

        private IEnumerator HandleEnterStrategy(Collider other, Rigidbody rb, float force,
            Transform aoeComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit != null) {
                unit.HealthComponent.DamageOwner(Damage);
                var vfx = MonoHelper.SpawnVfx(VfxType.DisruptTrigger, aoeComponentTransform.position,
                    Quaternion.Euler(90, 0, 0));
                vfx.AddComponent<SetParticleData>().Initialize(Duration, AreaOfEffectCircularRadius);
                Destroy(aoeComponentTransform.root.gameObject);
            }

            yield break;
        }

        // empty... =(
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) { }
    }
}