using System.Collections;
using Common;
using Projectiles;
using Stats;
using UI.Targeting;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class Rain : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            ShaderHelper.isCenterPosPlayerPos = false;
            yield return new WaitForSeconds(StartupTime);
            var updatedTargetLocation = MouseHelper.GetWorldPosition();
            SpawnAoEEffect(updatedTargetLocation);
            OnAbilityActivationFinished(Owner, this);
            ShaderHelper.isCenterPosPlayerPos = true;
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
        }

        private IEnumerator ApplyDamageOverTime(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) yield break;
            
            var totalDamage = Damage;
            var isMarked = unit.StatusComponent.Status.HasFlag(Status.Marked);
            if (isMarked)
            {
                totalDamage += 2;
                unit.StatusComponent.RemoveStatus(Status.Marked);
            }
            
            
            Debug.Log($"Rain has connected with a unit: {unit.name}.  The unit has a marked status of {isMarked}.\n" +
                      $"Base damage is {Damage}. Total Damage: {totalDamage}");
            
            unit.HealthComponent.TakeDamage(totalDamage * Time.deltaTime);
        }
        
        // empty... =(
        protected override void AbilityConnected(GameObject target, GameObject projectile = null) { }
    }
}