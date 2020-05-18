using System;
using System.Collections;
using Common;
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
            ShaderHelper.isCenterPosPlayerPos = true;
        }
        
        private GameObject SpawnGrenade(Vector3 targetLocation) {
            return MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
        }

        public override void AbilityConnected(GameObject other, GameObject projectile) {
            var colliderParams = new SphereParams(5f);
            var centerLocation = other.transform.position;
            var offset = centerLocation - projectile.transform.position;
            offset.y = 0;
            offset = offset.normalized * 2f;
            var targetLocation = centerLocation + offset;

            var pGo = new GameObject("Push Force")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    ForceStrategies.Strategies[ForceStrategies.Type.ForceAlongHeading],
                    AffectedFactions,
                    185)
                .gameObject
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    AoEAddMark,
                    AffectedFactions)
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