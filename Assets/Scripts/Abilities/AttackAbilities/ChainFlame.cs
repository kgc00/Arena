using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Data;
using Data.Types;
using Projectiles;
using State;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class ChainFlame : AttackAbility {
        private int iterations = 3;
        private float delayBetweenProjectiles = 0.5f;
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            OnAbilityActivationFinished(Owner, this);

            Vector3? updatedTargetLocation = targetLocation;

            for (int i = 0; i < iterations; i++) {
                updatedTargetLocation = Locator.GetClosestVisiblePlayerUnit(updatedTargetLocation.Value)?.position ?? targetLocation;
                var projectile = SpawnProjectile();
                InitializeProjectile(updatedTargetLocation.Value, projectile);
                yield return new WaitForSeconds(delayBetweenProjectiles);
            }

            updatedTargetLocation = Locator.GetClosestVisiblePlayerUnit(updatedTargetLocation.Value)?.position ?? updatedTargetLocation;
            var aoeProjectile = SpawnProjectile();
            InitializeAoEProjectile(updatedTargetLocation.Value, aoeProjectile);

            this.PostNotification(NotificationType.AbilityWillActivate, new UnitIntent(this,new TargetingData(TargetingBehavior.TargetLocation, updatedTargetLocation.Value), Owner));
            yield return new WaitForSeconds(delayBetweenProjectiles);
            this.PostNotification(NotificationType.AbilityDidActivate, new UnitIntent(this,new TargetingData(TargetingBehavior.TargetLocation, updatedTargetLocation.Value), Owner));

            ExecuteOnAbilityFinished();
        }

        /// <summary>
        /// Initialize the final projectile for this skill.  it has different requirements from other projectiles.
        /// Mainly, we want it to explode when it reaches it's destination.
        /// </summary>
        /// <param name="targetLocation"></param>
        /// <param name="aoeProjectile"></param>
        private void InitializeAoEProjectile(Vector3 targetLocation, GameObject aoeProjectile) {
            void OnConnected(GameObject other, GameObject projectile) {
                if (other.gameObject.CompareTag("Board")) {
                    Destroy(projectile.gameObject);
                    return;
                }
                
                var unit = other.GetUnitComponent();
                if (unit == null) return;

                if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;
                var proximityComponent = projectile.transform.root.GetComponent<ProximityComponent>() ??
                                         throw new Exception($"No Proximity component found on {name}");
                if (!proximityComponent.IsLive) return;
                SpawnAoEEffect(projectile.transform.position);
                proximityComponent.SetInactive();
            }

            var onConnectedCallback = new List<Action<GameObject, GameObject>> { OnConnected };
            var aoeCallback = new List<Action<Vector3>> { SpawnAoEEffect };

            aoeProjectile.GetComponent<ProjectileComponent>()
                .Initialize(targetLocation, onConnectedCallback, 10f);

            aoeProjectile.AddComponent<ProximityComponent>().Initialize(targetLocation, aoeCallback);
        }


        private void SpawnAoEEffect(Vector3 updatedTargetLocation) {
            var colliderParams = new SphereParams(AreaOfEffectRadius);
            var pGo = new GameObject("ChainFlame AoE Effect")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    updatedTargetLocation,
                    updatedTargetLocation,
                    ForceStrategies.Strategies[ForceStrategyType.ForceAlongHeading],
                    null, 
                    null,
                    AffectedFactions,
                    force: Force,
                    duration: Duration)
                .gameObject
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    updatedTargetLocation,
                    updatedTargetLocation,
                    ApplyDamage,
                    null, 
                    null,
                    AffectedFactions,
                    force: 0,
                    duration: Duration)
                .gameObject;

            Debug.Log("spawned AoE for Chain Flame");
        }



        private IEnumerator ApplyDamage(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;

            unit.HealthComponent.DamageOwner(Damage, this, Owner);
            var spawnPos = other.ClosestPoint(transform.position);
            MonoHelper.SpawnVfx(VfxType.PlayerImpact, spawnPos);
        }

        private void InitializeProjectile(Vector3 targetLocation, GameObject projectile) => projectile
            .GetComponent<ProjectileComponent>().Initialize(targetLocation, OnAbilityConnection, 10f);

        private GameObject SpawnProjectile() {
            var position = gameObject.transform.position;
            var forward = gameObject.transform.forward;

            // find offset
            var spawnPos = new Vector3(position.x, 1, position.z) + (forward * 2);

            // find rotation
            var relativeOffset = spawnPos - position;
            var yEuler = Quaternion.LookRotation(relativeOffset, Vector3.up).eulerAngles.y;
            var rotation = Quaternion.Euler(0, yEuler, 0);

            // instantiation
            return Instantiate(
                Resources.Load($"{Constants.PrefabsPath}Fireball Projectile VFX", typeof(GameObject)),
                spawnPos,
                rotation
                ) as GameObject;
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile = null) {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();


            if (hitGeometry) {
                Destroy(projectile);
                return;
            }

            if (unit == null || unit.Owner == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;
            unit.HealthComponent.DamageOwner(Damage, this, Owner);
            var projPos = projectile.transform.position;
            var offset = (other.transform.position - projPos) / 2;
            var spawnPos = projPos + offset;
            spawnPos.y = projPos.y;
            MonoHelper.SpawnVfx(VfxType.PlayerImpact, spawnPos);
            Destroy(projectile);
        }
    }
}