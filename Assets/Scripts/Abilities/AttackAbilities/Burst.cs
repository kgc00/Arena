using System.Collections;
using Common;
using Components;
using Data.Params;
using Data.Types;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class Burst : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.DidCastBurst);
            var updatedTargetLocation = MouseHelper.GetWorldPosition();
            SpawnGrenade(updatedTargetLocation);
            OnAbilityActivationFinished(Owner, this);
            ExecuteOnAbilityFinished();
        }

        private void SpawnGrenade(Vector3 targetLocation) {
            var go = MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
            var vfx = MonoHelper.SpawnVfx(VfxType.BurstProjectile, Vector3.zero);
            vfx.transform.SetParent(go.transform.Find("TipTransform") ?? go.transform, false);
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile = null) {
            // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
            var layerMask = LayerMask.GetMask("Units", "Board");
            if (layerMask != (layerMask | (1 << other.layer))) return;

            var colliderParams = new SphereParams(AreaOfEffectRadius);
            var projectilePosition = projectile == null ? Vector3.zero : projectile.transform.position;
            var centerLocation = other.GetComponent<Collider>().ClosestPoint(projectilePosition);
            var offset = centerLocation - projectilePosition;
            offset.y = 0;
            offset = offset.normalized * 2f;
            var targetLocation = centerLocation + offset;

            // should create some list that I can iterate through-
            // foreach AoEEffect => gameobject.AddComponent<AoEComponent>().Initialize(AoEEffect);
            var _ = new GameObject("Push Force")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    HandleEnterStrategy,
                    null,
                    null,
                    AffectedFactions,
                    Force,
                    Duration)
                .gameObject;
            var vfx = MonoHelper.SpawnVfx(VfxType.BurstImpact, centerLocation);
            vfx.AddComponent<SetParticleData>().Initialize(Duration, AreaOfEffectRadius);
            this.PostNotification(NotificationType.DidConnectBurst);
            Destroy(projectile);
        }

        private IEnumerator HandleEnterStrategy(Collider arg1, Rigidbody arg2, float arg3, Transform arg4) {
            yield return StartCoroutine(AoEAddMarkAndDealDamage(arg1, arg2, arg3, arg4));
            yield return StartCoroutine(ForceStrategies.Strategies[ForceStrategyType.ForceAlongHeading](arg1, arg2, arg3, arg4));
        }

        private IEnumerator AoEAddMarkAndDealDamage(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            unit.HealthComponent.DamageOwner(Damage);
            if (unit != null) {
                unit.StatusComponent.AddStatus(StatusType.Marked, 1);
            }
            yield break;
        }
    }
}