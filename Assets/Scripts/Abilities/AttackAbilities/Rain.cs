using System.Collections;
using System.Collections.Generic;
using Common;
using Data.Types;
using Projectiles;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class Rain : AttackAbility {
        private HashSet<Unit> _affectedUnits = new HashSet<Unit>();
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            var updatedTargetLocation = MouseHelper.GetWorldPosition();
            _affectedUnits.Clear();
            SpawnAoEEffect(updatedTargetLocation);
            OnAbilityActivationFinished(Owner, this);
        }

        private void SpawnAoEEffect(Vector3 updatedTargetLocation) {
            var colliderParams = new SphereParams(5f);
            var pGo = new GameObject("Rain AoE Effect")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    updatedTargetLocation,
                    updatedTargetLocation,
                    ApplyDamageOverTime,
                    null, 
                    StopApplyingDamage,
                    AffectedFactions,
                    force: 185,
                    duration: Duration)
                .gameObject;
            var vfx = MonoHelper.SpawnVfx(VfxType.RainScene, updatedTargetLocation, true);
            pGo.AddComponent<DestroyOtherOnDestroy>().LinkGameObject(vfx);
        }

        private IEnumerator StopApplyingDamage(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;

            if (!_affectedUnits.Contains(unit)) yield break;
            _affectedUnits.Remove(unit);
        }

        private IEnumerator ApplyDamageOverTime(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;

            unit.StatusComponent.AddStatus(StatusType.Marked);
            _affectedUnits.Add(unit);
            while (_affectedUnits.Contains(unit) && unit != null) {
                var damage = Damage * Time.deltaTime;
                Debug.Log($"Rain has connected with a unit: {unit.name}.  The unit has a marked status of {unit.StatusComponent.StatusType.HasFlag(StatusType.Marked)}.\n" +
                          $"Base damage is {damage}.");
                unit.HealthComponent.DamageOwner(damage, this, Owner);
                yield return new WaitForEndOfFrame();
            }
        }

        // empty... =(
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) { }
    }
}