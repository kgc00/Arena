using System.Collections;
using System.Collections.Generic;
using Common;
using Components;
using Data.Params;
using Data.Types;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class Rain : AttackAbility {
        private readonly HashSet<Unit> _affectedUnits = new HashSet<Unit>();
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.DidCastRain);
            OnAbilityActivationFinished(Owner, this);
            _affectedUnits.Clear();
            SpawnAoEEffect(targetLocation);
            ExecuteOnAbilityFinished();
        }

        private void SpawnAoEEffect(Vector3 updatedTargetLocation) {
            var colliderParams = new SphereParams(AreaOfEffectCircularRadius);
            var _ = new GameObject("Rain AoE Effect")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    updatedTargetLocation,
                    updatedTargetLocation,
                    ApplyDamageOverTime,
                    null,
                    StopApplyingDamage,
                    AffectedFactions,
                    force: default,
                    duration: Duration,
                    HandleAoEComponentDestroyed
                    )
                .gameObject;
            var vfx = MonoHelper.SpawnVfx(VfxType.RainScene, updatedTargetLocation, true);
            vfx.AddComponent<SetParticleData>().Initialize(Duration, AreaOfEffectCircularRadius);
        }

        private void HandleAoEComponentDestroyed() {
            this.PostNotification(NotificationType.RainDidFinish);
            foreach (var unit in _affectedUnits) {
                if (unit == null) continue;
                var vfx = unit.GetComponentInChildren<ModifyPositionAndTagVFX>();
                if (vfx) {
                    Destroy(vfx.gameObject);
                }
            }
            _affectedUnits.Clear();
        }

        private IEnumerator StopApplyingDamage(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;
            var vfx = unit.GetComponentInChildren<ModifyPositionAndTagVFX>();
            if (vfx) {
                Destroy(vfx.gameObject);
            }

            if (!_affectedUnits.Contains(unit)) yield break;
            _affectedUnits.Remove(unit);
        }

        // private IEnumerator HandleEnter(Collider other, Rigidbody rigidBody, float Force,
        //     Transform forceComponentTransform) {
        //     if (Owner.gameObject == null) yield break;
        //     yield return StartCoroutine(AoEAddMarkAndDealDamage(other, rigidBody, Force, forceComponentTransform));
        //     if (Owner.gameObject == null) yield break;
        //     yield return StartCoroutine(ApplyDamageOverTime(other, rigidBody, Force, forceComponentTransform));
        //
        // }

        private IEnumerator ApplyDamageOverTime(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;
            if (_affectedUnits.Contains(unit)) yield break;

            _affectedUnits.Add(unit);
            foreach (var cb in OnAbilityConnection)
                cb(other.gameObject, null);

            MonoHelper.SpawnVfx(VfxType.RainImpact, unit.transform.position).transform.SetParent(unit.transform);
            while (_affectedUnits.Contains(unit) && unit != null) {
                var damage = Damage / Duration * Time.deltaTime;
                unit.HealthComponent.DamageOwner(damage, this, Owner);
                yield return new WaitForEndOfFrame();
            }
        }

        // empty... =(
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) { }
    }
}