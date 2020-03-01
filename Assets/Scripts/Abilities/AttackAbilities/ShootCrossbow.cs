using System;
using Projectiles;
using Stats;
using UnityEngine;

namespace Abilities.AttackAbilities
{
    public class ShootCrossbow : AttackAbility
    {
        private void OnEnable()
        {
            Damage = 1f;
        }

        public override void Activate(Vector3 targetLocation)
        {
            var projectile = SpawnProjectile();
            InitializeProjectile(targetLocation, projectile);
        }

        private void InitializeProjectile(Vector3 targetLocation, GameObject projectile)
        {
            if (projectile == null) return;
            
            var relativePosition = targetLocation - transform.position;
            projectile.GetComponent<ProjectileComponent>().Initialize(relativePosition, OnAbilityConnected, 10f);
        }

        private GameObject SpawnProjectile()
        {
            var position = gameObject.transform.position;
            var forward = gameObject.transform.forward;
            
            // find offset
            var spawnPos = new Vector3(position.x, 1, position.z) + (forward * 2);
            
            // find rotation
            var yEuler = Quaternion.LookRotation(spawnPos + forward * 3).eulerAngles.y;
            var rotation = Quaternion.Euler(0, yEuler,0);

            // instantiation
            return  Instantiate(
                Resources.Load("Projectiles/Projectile", typeof(GameObject)),
                spawnPos, 
                rotation
                ) as GameObject;
        }

        public override void OnAbilityConnected(GameObject targetedUnit)
        {
            var healthComponent = targetedUnit.GetComponent<HealthComponent>();
            if (healthComponent == null) return;

            healthComponent.AdjustHealth(-Damage);
        }
    }
}