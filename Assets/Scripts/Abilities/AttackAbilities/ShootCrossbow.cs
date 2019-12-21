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
            if (projectile != null)
            {
                var relativePosition = targetLocation - transform.position;
                projectile.GetComponent<ProjectileComponent>().Initialize(relativePosition, OnAbilityConnected, 10f);
            }
        }

        private GameObject SpawnProjectile()
        {
            var position = Owner.transform.position;
            var spawnLocationWithOffset = new Vector3(position.x, 1, position.z) + (Owner.transform.forward * 2);

            var projectile = Instantiate(Resources.Load("Projectiles/Projectile", typeof(GameObject)),
                spawnLocationWithOffset, Quaternion.LookRotation(spawnLocationWithOffset)) as GameObject;
            return projectile;
        }

        public override void OnAbilityConnected(GameObject targetedUnit)
        {
            var healthComponent = targetedUnit.GetComponent<HealthComponent>();
            if (healthComponent == null) return;
            
            healthComponent.AdjustHealth(-Damage);
        }
    }
}