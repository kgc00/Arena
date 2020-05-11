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
            var pullGo = HandlePullEffect();

            yield return new WaitForSeconds(preFireDelay);
            Destroy(pullGo);
            // MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
        }

        private GameObject HandlePullEffect() {
            // player may have moved mouse during startup time, update target location 
            var targetLocation = MouseHelper.GetWorldPosition();

            var startLocation = Owner.transform.position;
            var heading = targetLocation - startLocation;
            var distance = heading.magnitude;

            var overgroundDirection = heading / distance;
            overgroundDirection.y = 0f; // remove height from caluclations
            overgroundDirection = overgroundDirection.normalized;

            var centerLocation = (overgroundDirection * (Range / 2)) + startLocation;
            var bounds = new Vector3(Range / 4, 1, Range);

            var offset = overgroundDirection * 1.5f;
            centerLocation += offset;
            targetLocation += (overgroundDirection * Range); //  ensure the pull component's z always points away from player

            return new GameObject("Pull Force").AddComponent<PullComponent>()
                .Initialize(185f, bounds, centerLocation, targetLocation, AffectedFactions);
        }

        public override void AbilityConnected(GameObject target, GameObject projectile = null)
        {
            // throw new System.NotImplementedException();
        }
    }
}