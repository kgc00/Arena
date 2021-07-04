using System.Collections;
using Common;
using Data.Types;
using Projectiles;
using UI.Targeting;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class Rain : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            var updatedTargetLocation = MouseHelper.GetWorldPosition();
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
                    null,
                    ApplyDamageOverTime,
                    AffectedFactions,
                    185,
                    Duration)
                .gameObject;
            var vfx = MonoHelper.SpawnVfx(VfxType.RainScene, updatedTargetLocation, true);
        }

        private IEnumerator ApplyDamageOverTime(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;

            unit.StatusComponent.AddStatus(StatusType.Marked);

            // scale damage applied to match time passed
            var damage = Damage * Time.deltaTime;

            Debug.Log($"Rain has connected with a unit: {unit.name}.  The unit has a marked status of {unit.StatusComponent.StatusType.HasFlag(StatusType.Marked)}.\n" +
                      $"Base damage is {damage}.");
            unit.HealthComponent.DamageOwner(damage, this, Owner);
        }

        // empty... =(
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) { }
    }
}