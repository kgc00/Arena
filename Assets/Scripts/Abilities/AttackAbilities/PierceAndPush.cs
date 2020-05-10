using System.Collections;
using Projectiles;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities
{
    public class PierceAndPush : AttackAbility
    {
        private float preFireDelay = 0.5f;
        public override void AbilityActivated(Vector3 targetLocation)
        {
            StartCoroutine(HandleActivation(targetLocation));
        }

        private IEnumerator HandleActivation(Vector3 targetLocation)
        {
            yield return new WaitForSeconds(StartupTime);
            // player may have moved mouse during startup time, update target location 
            targetLocation = MouseHelper.GetWorldPosition();
            
            var startLocation = Owner.transform.position;
            Vector3 heading = targetLocation - startLocation;
            Debug.Log($"heading {heading}");   
            
            var distance = heading.magnitude;                
            Debug.Log($"distance {distance}");
            
            var overgroundDirection =  heading / distance;
            overgroundDirection.y = 0f; // remove height from caluclations
            var center = (overgroundDirection * (Range / 2)) + startLocation;
            Debug.Log(center);
            
            var bounds = new Vector3(Range / 4, 1, Range);                
            Debug.Log($"bounds {bounds}");
            
            var offset = overgroundDirection * 1.5f;
            center += offset;
            targetLocation += (overgroundDirection * Range); // point towards target. always must be far enough away that the pull component orients away from player
            
            var go = new GameObject("Pull Force").AddComponent<PullComponent>()
                                                                 .Initialize(185f, bounds, center, targetLocation, AffectedFactions);
            
            yield return new WaitForSeconds(preFireDelay);
            Destroy(go);
            // MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
        }

        public override void AbilityConnected(GameObject target, GameObject projectile = null)
        {
            // throw new System.NotImplementedException();
        }
    }
}