using System;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;

namespace Utils
{
    public class MonoHelper : MonoBehaviour
    {
        #region Projectile
        public static GameObject SpawnProjectile(GameObject owner, Vector3 targetLocation, List<Action<GameObject, 
                                                GameObject>> onAbilityConnection, float projectileSpeed = 10f)
        {
            var projectile = SpawnProjectile(owner);
            
            projectile.GetComponent<ProjectileComponent>()
                .Initialize(targetLocation, onAbilityConnection, projectileSpeed);
            
            return projectile;
        }

        private static GameObject SpawnProjectile(GameObject Owner)
        {
            var position = Owner.transform.position;
            var forward = Owner.transform.forward;
            
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
        #endregion
    }
}