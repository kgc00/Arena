using Enums;
using Projectiles;
using Stats;
using Units;
using UnityEngine;
namespace Abilities.AttackAbilities
{
    public class Prey : AttackAbility
    {
        public override void AbilityActivated(Vector3 targetLocation)
        {
            var projectile = SpawnProjectile();
            InitializeProjectile(targetLocation, projectile);
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

        
        public override void AbilityConnected(GameObject other, GameObject projectile)
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

            var totalDamage = Damage;
            var isMarked = unit.StatusComponent.Status.HasFlag(Status.Marked);
            if (isMarked)
            {
                totalDamage += 2;
                unit.StatusComponent.RemoveStatus(Status.Marked);
            }

            Debug.Log($"Prey has connected with a unit: {unit.name}.  The unit has a marked status of {isMarked}.\n" +
                      $"Base damage is {Damage}. Total Damage: {totalDamage}");
            
            unit.HealthComponent.AdjustHealth(totalDamage);
            Destroy(projectile);
        }
    }
}