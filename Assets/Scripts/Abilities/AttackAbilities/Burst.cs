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
            ShaderHelper.isCenterPosPlayerPos = false;
            yield return new WaitForSeconds(StartupTime);
            var updatedTargetLocation = MouseHelper.GetWorldPosition();
            var grenade = SpawnGrenade(updatedTargetLocation);
            OnAbilityActivationFinished(Owner, this);
            ShaderHelper.isCenterPosPlayerPos = true;
        }
        
        private GameObject SpawnGrenade(Vector3 targetLocation) {
            return MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile) {
            var colliderParams = new SphereParams(5f);
            var centerLocation = other.GetComponent<Collider>().ClosestPoint(projectile.transform.position);
            var offset = centerLocation - projectile.transform.position;
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
                    AffectedFactions,
                    default,
                    Duration)
                .gameObject;
            
            Destroy(projectile);
        }

        private IEnumerator AoEAddMark(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit= other.transform.root.GetComponentInChildren<Unit>();
            if (unit != null) {
                StatusHelper.AddMark(unit);
            }
            yield break;
        }
    }
}