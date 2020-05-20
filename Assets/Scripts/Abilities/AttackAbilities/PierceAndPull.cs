using System.Collections;
using System.Collections.Generic;
using Common;
using Controls;
using Enums;
using Projectiles;
using Stats;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class PierceAndPull : AttackAbility {
        private float preFireDelay = 0.5f;

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            var pullGo = HandlePullEffect();
            Owner.inputModifierComponent.AddModifier(InputModifier.CannotMove).AddModifier(InputModifier.CannotRotate);

            yield return new WaitForSeconds(preFireDelay);
            Destroy(pullGo);
            MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
            Owner.inputModifierComponent.RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate);
            
            onAbilityActivationFinished(Owner, this);
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
            targetLocation += (overgroundDirection * Range); /* ensure the pull component's z 
                                                                always points away from player */

            var colliderParams = new BoxParams(bounds);
            
            return new GameObject("Pull Force").AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    ForceStrategies.Strategies[ForceStrategies.Type.ForceAlongLocalX],
                    null,
                    AffectedFactions, 
                    -185f)
                .gameObject;
        }

        public override void AbilityConnected(GameObject other, GameObject projectile = null) {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();

            if (hitGeometry) {
                Destroy(projectile);
                return;
            }

            if (unit == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;

            var totalDamage = Damage;
            var isMarked = unit.StatusComponent.Status.HasFlag(Status.Marked);
            if (isMarked) {
                totalDamage += 2;
                unit.StatusComponent.RemoveStatus(Status.Marked);
            }

            Debug.Log($"Pierce has connected with a unit: {unit.name}.  The unit has a marked status of {isMarked}.\n" +
                      $"Base damage is {Damage}. Total Damage is {totalDamage}");

            unit.HealthComponent.TakeDamage(totalDamage);
        }
    }
}