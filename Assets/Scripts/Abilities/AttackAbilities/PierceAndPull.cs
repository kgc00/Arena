using System.Collections;
using System.Collections.Generic;
using Common;
using Controls;
using Data.Types;
using DG.Tweening;
using Projectiles;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class PierceAndPull : AttackAbility {
        private float preFireDelay = 0.5f;

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            var relativePos = targetLocation - transform.position;
            var lookRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            var weaponTransform = gameObject.GetWeaponTransform();
            Destroy( MonoHelper.SpawnVfx(VfxType.PiercePullStartup, weaponTransform.position, lookRotation), StartupTime + preFireDelay);
            Owner.InputModifierComponent.AddModifier(InputModifier.CannotMove).AddModifier(InputModifier.CannotRotate);
            yield return new WaitForSeconds(StartupTime);

            var pullGo = HandlePullEffect(targetLocation);
            yield return new WaitForSeconds(preFireDelay);
            
            Destroy(pullGo);
            MonoHelper.SpawnVfx(VfxType.PiercePullLaunch, weaponTransform.position, lookRotation);
            var projectile = MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, ProjectileSpeed);
            var tipTransform = projectile.transform.Find("TipTransform") ?? projectile.transform;
            var projectileVFX = MonoHelper.SpawnVfx(VfxType.PiercePullProjectile, tipTransform.position);
            projectileVFX.transform.SetParent(tipTransform);

            Owner.InputModifierComponent
                .RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate);
            OnAbilityActivationFinished(Owner, this);
        }

        private GameObject HandlePullEffect(Vector3 targetLocation) {
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

            var angle = Mathf.Atan2(heading.x, heading.z) * Mathf.Rad2Deg;
            var lookRotation = Quaternion.Euler(0, angle, 0);
            
            var vfxCenter = centerLocation;
            vfxCenter.y = bounds.y / 2; 
            var vfx = MonoHelper.SpawnVfx(VfxType.PiercePullForce, vfxCenter, lookRotation);
            vfx.transform.localScale = bounds;
            
            var colliderParams = new BoxParams(bounds);
            
            return new GameObject("Pull Force").AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    centerLocation,
                    targetLocation,
                    ForceStrategies.Strategies[ForceStrategyType.ForceAlongLocalX],
                    null, 
                    null,
                    AffectedFactions, 
                    force: -185f)
                .gameObject;
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile = null) {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();

            if (hitGeometry) {
                Destroy(projectile);
                return;
            }

            if (unit == null || unit.Owner == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;

            var totalDamage = Damage;
            var isMarked = unit.StatusComponent.StatusType.HasFlag(StatusType.Marked);
            if (isMarked) {
                totalDamage += 2;
                unit.StatusComponent.RemoveStatus(StatusType.Marked);
                MonoHelper.SpawnVfx(VfxType.MarkExplosion, unit.transform.position);
            }

            Debug.Log($"Pierce has connected with a unit: {unit.name}.  The unit has a marked status of {isMarked}.\n" +
                      $"Base damage is {Damage}. Total Damage is {totalDamage}");
            
            unit.HealthComponent.DamageOwner(Damage, this, Owner);

        }
    }
}