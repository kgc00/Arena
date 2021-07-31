using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Data.Types;
using Projectiles;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class ChainFlame : AttackAbility {
        private int iterations = 3;
        private float delayBetweenProjectiles = 0.5f;
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            OnAbilityActivationFinished(Owner, this);

            Vector3? updatedTargetLocation = targetLocation;

            for (int i = 0; i < iterations; i++) {
                updatedTargetLocation = Locator.GetClosestPlayerUnit(updatedTargetLocation.Value)?.position;
                if (updatedTargetLocation == null) yield break;
                var projectile = SpawnProjectile();
                InitializeProjectile(updatedTargetLocation.Value, projectile);
                yield return new WaitForSeconds(delayBetweenProjectiles);
            }

            updatedTargetLocation = Locator.GetClosestPlayerUnit(updatedTargetLocation.Value).position;
            var aoeProjectile = SpawnProjectile();
            InitializeAoEProjectile(updatedTargetLocation.Value, aoeProjectile);

            MonoHelper.SpawnEnemyIndicator(updatedTargetLocation.Value, AreaOfEffectRadius, aoeProjectile);

            yield return new WaitForSeconds(delayBetweenProjectiles);

            OnAbilityFinished(Owner, this);
        }

        /// <summary>
        /// Initialize the final projectile for this skill.  it has different requirements from other projectiles.
        /// Mainly, we want it to explode when it reaches it's destination.
        /// </summary>
        /// <param name="targetLocation"></param>
        /// <param name="aoeProjectile"></param>
        private void InitializeAoEProjectile(Vector3 targetLocation, GameObject aoeProjectile) {
            void OnConnected(GameObject other, GameObject projectile) {
                var unit = other.GetUnitComponent();
                if (!unit) {
                    Destroy(projectile.gameObject);
                    return;
                }

                if (AffectedFactions.Contains(unit.Owner.ControlType)) {
                    var proximityComponent = projectile.transform.root.GetComponent<ProximityComponent>() ??
                                             throw new Exception($"No Proximity component found on {name}");
                    if (proximityComponent.IsLive) {
                        SpawnAoEEffect(projectile.transform.position);
                        proximityComponent.SetInactive();
                    }
                }
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
                Resources.Load($"{Constants.PrefabsPath}Projectile", typeof(GameObject)),
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
            Destroy(projectile);
        }
    }
}