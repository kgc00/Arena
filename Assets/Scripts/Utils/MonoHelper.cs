using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UI;
using Units;
using UnityEngine;
using Types = VFX.Types;

namespace Utils
{
    public class MonoHelper : MonoBehaviour {

        #region Indicator
        public static GameObject SpawnEnemyIndicator(Vector3 pos, float radius, float lifetime) =>
            Instantiate(Resources.Load<GameObject>("VFX/EnemyIndicator"))
                .GetComponent<EnemyIndicator>()
                .Initialize(pos, radius, lifetime)
                .gameObject;
        
        
        public static GameObject SpawnEnemyIndicator(Vector3 pos, float radius, GameObject projectile) =>
            Instantiate(Resources.Load<GameObject>("VFX/EnemyIndicator"))
                .GetComponent<EnemyIndicator>()
                .Initialize(pos, radius, projectile)
                .gameObject;
        #endregion

        #region VFX
        public static GameObject SpawnVfx(Types type, Vector3 pos) => 
            Instantiate(TypeToVfx(type), pos, Quaternion.Euler(-90,0,0));
        
        private static GameObject TypeToVfx(Types type) {
            var path = ResourcePathFromType(type) ??
                       throw new Exception($"Unable to location {type} in ResourcePathFromType");

            return Resources.Load<GameObject>(path);
        }

        private static string ResourcePathFromType(Types type) {
            string s = "";
            switch (type) {
                case Types.EnemySpawnIndicator:
                    s = "VFX/EnemySpawn";
                    break;
                case Types.EnemyAoEIndicator:
                    s = "VFX/EnemyAoEIndicator";
                    break;
                case Types.ExplosionRed:
                    s = "VFX/ExplosionRed";
                    break;
            }

            return s;
        }

        #endregion
        
        #region Projectile
        public static GameObject SpawnProjectile(GameObject owner, Vector3 targetLocation, List<Action<GameObject, 
                                                GameObject>> onAbilityConnection, float projectileSpeed = default)
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

        #region Coroutine
        #endregion
    }
}