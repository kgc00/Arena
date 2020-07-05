using System.Collections;
using Enums;
using Projectiles;
using Units;
using UnityEngine;

namespace Abilities.AttackAbilities {
    public class ChainFlame : AttackAbility{
        public override IEnumerator AbilityActivated(Vector3 targetLocation)
        {
            if (Cooldown.IsOnCooldown) 
                yield break;

            var projectile = SpawnProjectile();
            InitializeProjectile(targetLocation, projectile);

            OnAbilityActivationFinished(Owner, this);
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

        protected override void AbilityConnected(GameObject other, GameObject projectile)
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