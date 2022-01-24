using System;
using System.Collections;
using Common;
using Data.Types;
using Projectiles;
using UI.Targeting;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class Burst : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
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

            var colliderParams = new SphereParams(5f);
            var projectilePosition = projectile == null ? Vector3.zero : projectile.transform.position;
            var centerLocation = other.GetComponent<Collider>().ClosestPoint(projectilePosition);
            var offset = centerLocation - projectilePosition;
            offset.y = 0;
            offset = offset.normalized * 2f;
            var targetLocation = centerLocation + offset;

            // should create some list that I can iterate through-
            // foreach AoEEffect => gameobject.AddComponent<AoEComponent>().Initialize(AoEEffect);

            // Requires refactoring this logic out into a more generic model which would live on abilities themselves
            // May also need to have the AoEComponent inherit from Ability or something.
            var pGo = new GameObject("Push Force")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    ForceStrategies.Strategies[ForceStrategyType.ForceAlongHeading],
                    null,
                    null,
                    AffectedFactions,
                    185,
                    Duration)
                .gameObject
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    AoEAddMark,
                    null,
                    null,
                    AffectedFactions,
                    default,
                    Duration)
                .gameObject;
            MonoHelper.SpawnVfx(VfxType.BurstImpact, centerLocation);

            Destroy(projectile);
        }

        private IEnumerator AoEAddMark(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit != null) {
                StatusHelper.AddMark(unit);
            }
            yield break;
        }
    }
}