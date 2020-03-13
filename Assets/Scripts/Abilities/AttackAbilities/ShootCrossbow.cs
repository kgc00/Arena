using System;
using Projectiles;
using Stats;
using UnityEngine;

namespace Abilities.AttackAbilities
{
    public class ShootCrossbow : AttackAbility
    {
        private Vector3 debugCurrentTarget;
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
            
            projectile.GetComponent<ProjectileComponent>().Initialize(targetLocation, OnAbilityConnected, 10f);
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

        public override void OnAbilityConnected(GameObject targetedUnit)
        {
            Debug.Log("triggered");
            var healthComponent = targetedUnit.GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                Destroy(gameObject, 0.05f);
                return;
            }

            healthComponent.AdjustHealth(-Damage);
        }
    }
}