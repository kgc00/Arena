using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Data.Pickups;
using Data.Types;
using Projectiles;
using Status;
using UI;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils {
    public class MonoHelper : MonoBehaviour {
        #region Drops
        public static GameObject SpawnDrop(DropType dropType, Vector3 pos) {
            var path = "";
            switch (dropType) {
                case DropType.HealthPickupSmall:
                    path = $"{Constants.PrefabsPath}Health Pickup Small";
                    break;
                case DropType.HealthPickupLarge:
                    path = $"{Constants.PrefabsPath}Health Pickup Large";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dropType), dropType, null);
            }
            
            return Instantiate(Resources.Load<GameObject>(path), pos, Quaternion.identity);
        }
        #endregion 
        
        #region VFX
        public static GameObject SpawnVfx(VfxType vfxType, Vector3 pos, bool identityRot = false) =>
            Instantiate(TypeToVfx(vfxType), pos, identityRot ? Quaternion.identity : Quaternion.Euler(-90, 0, 0));
            
        public static GameObject SpawnVfx(VfxType vfxType, Vector3 pos, Quaternion rotation) =>
            Instantiate(TypeToVfx(vfxType), pos, rotation);

        public static Material LoadMaterial(MaterialType materialType) =>
            Resources.Load<Material>(MaterialResourcePaths[materialType]);

        private static GameObject TypeToVfx(VfxType vfxType) {
            var path = ResourcePathFromType(vfxType) ??
                       throw new Exception($"Unable to location {vfxType} in ResourcePathFromType");

            Debug.Log(path);
            return Resources.Load<GameObject>(path);
        }

        private static string ResourcePathFromType(VfxType vfxType) {
            string s = "";
            switch (vfxType) {
                case VfxType.EnemySpawnIndicator:
                    s = $"{Constants.PrefabsPath}EnemySpawn";
                    break;
                case VfxType.EnemyAoEIndicator:
                    s = $"{Constants.PrefabsPath}EnemyAoEIndicator";
                    break;
                case VfxType.ExplosionRed:
                    s = $"{Constants.PrefabsPath}ExplosionRed";
                    break;
                case VfxType.Poof:
                    s = $"{Constants.PrefabsPath}Poof VFX";
                    break;
                case VfxType.BurstProjectile:
                    s = $"{Constants.PrefabsPath}BurstProjectile";
                    break;
                case VfxType.BurstImpact:
                    s = $"{Constants.PrefabsPath}BurstImpactVFX";
                    break;
                case VfxType.RainScene:
                    s = $"{Constants.PrefabsPath}Rain VFX Scene";
                    break;
                case VfxType.PiercePullStartup:
                    s = $"{Constants.PrefabsPath}Pierce And Pull Startup VFX";
                    break;
                case VfxType.PiercePullLaunch:
                    s = $"{Constants.PrefabsPath}Pierce And Pull Launch VFX";
                    break;
                case VfxType.PiercePullProjectile:
                    s = $"{Constants.PrefabsPath}Pierce And Pull Projectile VFX";
                    break;
                case VfxType.PiercePullForce:
                    s = $"{Constants.PrefabsPath}Pierce And Pull Force VFX";
                    break;
                case VfxType.PiercePullImpact:
                    s = $"{Constants.PrefabsPath}Pierce And Pull Impact VFX";
                    break;
                case VfxType.Mark:
                    s = $"{Constants.PrefabsPath}Mark VFX";
                    break;
                case VfxType.MarkExplosion:
                    s = $"{Constants.PrefabsPath}Mark Explosion VFX";
                    break;
                case VfxType.LevelUp:
                    s = $"{Constants.PrefabsPath}Level Up VFX";
                    break;
                case VfxType.EnemyImpact:
                    s = $"{Constants.PrefabsPath}Enemy Impact VFX";
                    break;
                case VfxType.PlayerImpact:
                    s = $"{Constants.PrefabsPath}Player Impact VFX";
                    break;
                case VfxType.RainImpact:
                    s = $"{Constants.PrefabsPath}Rain Impact VFX";
                    break;
                case VfxType.Roar:
                    s = $"{Constants.PrefabsPath}Roar VFX";
                    break;
                case VfxType.DropSpawn:
                    s = $"{Constants.PrefabsPath}DropSpawnVFX";
                    break;
                case VfxType.HealPickup:
                    s = $"{Constants.PrefabsPath}HealPickupVFX";
                    break;
            }

            return s;
        }

        private static readonly Dictionary<MaterialType, string> MaterialResourcePaths = new Dictionary<MaterialType, string> {
            {MaterialType.MarkOutline, $"{Constants.MaterialsPath}MarkOutline"}
        };
        
        #endregion

        #region Projectile
        public static GameObject SpawnProjectile(GameObject owner, Vector3 targetLocation,
            List<Action<GameObject, GameObject>> onAbilityConnection, float projectileSpeed = default,
            float triggerWidthOverride = -1) {
            var projectile = SpawnProjectile(owner);

            projectile.GetComponent<ProjectileComponent>()
                .Initialize(targetLocation, onAbilityConnection, projectileSpeed, Int16.MaxValue, triggerWidthOverride);

            return projectile;
        }

        private static GameObject SpawnProjectile(GameObject Owner) {
            var position = Owner.transform.position;
            var forward = Owner.transform.forward;

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
        #endregion

        #region Coroutine
        #endregion
    }
}
