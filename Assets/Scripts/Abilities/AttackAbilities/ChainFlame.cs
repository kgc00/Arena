using System.Collections;
using Common;
using Enums;
using Projectiles;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class ChainFlame : AttackAbility {
        private int iterations = 3;
        private float delayBetweenProjectiles = 0.15f;
        public override IEnumerator AbilityActivated(Vector3 targetLocation)
        {
            yield return new WaitForSeconds(StartupTime);
            OnAbilityActivationFinished(Owner, this);

            for (int i = 0; i < iterations; i++) {
                var projectile = SpawnProjectile();
                InitializeProjectile(targetLocation, projectile);
                yield return new WaitForSeconds(delayBetweenProjectiles);
            }

            SpawnAoEEffect(targetLocation);
            
            yield return new WaitForSeconds(delayBetweenProjectiles);
            
            OnAbilityFinished(Owner, this);
        }

        
        private void SpawnAoEEffect(Vector3 updatedTargetLocation) {
            var colliderParams = new SphereParams(3f);
            var pGo = new GameObject("Rain AoE Effect")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    updatedTargetLocation,
                    updatedTargetLocation,
                    ForceStrategies.Strategies[ForceStrategies.Type.ForceAlongHeading],
                    null,
                    AffectedFactions,
                    185,
                    Duration)
                .gameObject
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    updatedTargetLocation,
                    updatedTargetLocation,
                    ApplyDamage,
                    null,
                    AffectedFactions,
                    185,
                    Duration)
                .gameObject;
        }



        private IEnumerator ApplyDamage(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;

            unit.HealthComponent.DamageOwner(Damage, this, Owner);
        }

        private void InitializeProjectile(Vector3 targetLocation, GameObject projectile)
        {
            if (projectile == null) return;
            
            projectile.GetComponent<ProjectileComponent>().Initialize(targetLocation, OnAbilityConnection, 10f);
        }

        private GameObject SpawnProjectile()
        {
            var position = gameObject.transform.position;
            var forward = gameObject.transform.forward;
            
            // find offset
            var spawnPos = new Vector3(position.x, 1, position.z) + (forward * 2);
            
            // find rotation
            var relativeOffset = spawnPos - position;
            var yEuler = Quaternion.LookRotation(relativeOffset, Vector3.up).eulerAngles.y;
            var rotation = Quaternion.Euler(0, yEuler,0);

            // instantiation
            return  Instantiate(
                Resources.Load("Projectiles/Projectile", typeof(GameObject)),
                spawnPos, 
                rotation
                ) as GameObject;
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile = null)
        {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            
            
            if (hitGeometry)
            {
                Destroy(projectile);
                return;
            }

            if (unit == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;
            unit.HealthComponent.DamageOwner(Damage, this, Owner);
            Destroy(projectile);
        }
    }
}