using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Components;
using Data;
using Data.Params;
using Data.Types;
using State;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class Disrupt : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            Vector3? updatedTargetLocation = targetLocation;
            Rigidbody targetRB = Locator.GetClosestVisiblePlayerUnit(targetLocation)?.GetComponent<Rigidbody>();
            if (targetRB == null) yield break;
            updatedTargetLocation = targetRB.transform.position + (targetRB.velocity * 50 * Time.deltaTime);
            if (!updatedTargetLocation.HasValue) yield break;
            
            this.PostNotification(NotificationType.AbilityWillActivate, new UnitIntent(this,new TargetingData(TargetingBehavior.TargetLocation, updatedTargetLocation.Value), Owner));
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.AbilityDidActivate, new UnitIntent(this,new TargetingData(TargetingBehavior.TargetLocation, updatedTargetLocation.Value), Owner));
            this.PostNotification(NotificationType.DidCastDisrupt);
            OnAbilityActivationFinished(Owner, this);
            SpawnAoEEffect(updatedTargetLocation.Value);
            ExecuteOnAbilityFinished();
        }

        private void SpawnAoEEffect(Vector3 targetLocation) {
            var colliderParams = new SphereParams(AreaOfEffectRadius);
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
            var vfx = MonoHelper.SpawnVfx(VfxType.DisruptStartup, targetLocation);
            vfx.AddComponent<SetParticleData>().Initialize(Duration, AreaOfEffectRadius);
            vfx.transform.SetParent(aoeGo.transform);
        }

        private IEnumerator HandleEnterStrategy(Collider other, Rigidbody rb, float force, Transform aoeComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit != null) {
                unit.HealthComponent.DamageOwner(Damage);
                var vfx = MonoHelper.SpawnVfx(VfxType.DisruptTrigger, aoeComponentTransform.position, Quaternion.Euler(90,0,0));
                vfx.AddComponent<SetParticleData>().Initialize(Duration, AreaOfEffectRadius);
                Destroy(aoeComponentTransform.root.gameObject);
            }
            yield break;
        }

        // empty... =(
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) { 

        }
    }
}