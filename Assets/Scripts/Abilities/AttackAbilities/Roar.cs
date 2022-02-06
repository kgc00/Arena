using System.Collections;
using System.Linq;
using Common;
using Data.Types;
using Projectiles;
using Units;
using UnityEngine;
using Utils;
using static Utils.MathHelpers;

namespace Abilities.AttackAbilities {
    public class Roar : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            OnAbilityActivationFinished(Owner, this);
            Destroy(MonoHelper.SpawnVfx(VfxType.Roar, gameObject.transform.position), Duration - StartupTime);
            var colliderParams = new SphereParams(AreaOfEffectRadius);
            var _ = new GameObject("Pull Force").AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    Owner.transform.position,
                    Owner.transform.position,
                    HandleEnterEnterEffect,
                    null,
                    null,
                    AffectedFactions,
                    Force, 
                    Duration - StartupTime)
                .gameObject;
            yield return new WaitForSeconds(Duration);
            ExecuteOnAbilityFinished();
        }

        private IEnumerator HandleEnterEnterEffect(Collider arg1, Rigidbody arg2, float arg3, Transform arg4) {
            var unit = arg1.gameObject.GetUnitComponent();
            if (unit != null) {
                AbilityConnected(unit.gameObject, null);
                foreach (var cb in OnAbilityConnection) {
                    cb(unit.gameObject, null);
                }
            }

            yield return StartCoroutine(
                ForceStrategies.Strategies[ForceStrategyType.ForceAlongHeading](arg1, arg2, arg3, arg4));
        }

        protected override void AbilityConnected(GameObject target, GameObject projectile = null) {
            target.GetUnitComponent().HealthComponent.DamageOwner(Damage, this, Owner);
        }
    }
}