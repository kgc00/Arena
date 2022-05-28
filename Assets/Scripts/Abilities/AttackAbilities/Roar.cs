using System.Collections;
using System.Linq;
using Common;
using Components;
using Data;
using Data.Params;
using Data.Types;
using State;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;
using static Utils.MathHelpers;

namespace Abilities.AttackAbilities {
    public class Roar : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            var ownerPos = Owner.transform.position;
            this.PostNotification(NotificationType.AbilityWillActivate, new UnitIntent(this, new TargetingData(TargetingBehavior.TargetLocation, ownerPos), Owner));
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.AbilityDidActivate, new UnitIntent(this, new TargetingData(TargetingBehavior.TargetLocation, ownerPos), Owner));
            this.PostNotification(NotificationType.DidCastRoar);
            OnAbilityActivationFinished(Owner, this);
            Destroy(MonoHelper.SpawnVfx(VfxType.Roar, ownerPos), Duration - StartupTime);
            var colliderParams = new SphereParams(AreaOfEffectCircularRadius);
            var _ = new GameObject("Pull Force").AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    ownerPos,
                    ownerPos,
                    HandleEnterEnterEffect,
                    null,
                    null,
                    AffectedFactions,
                    Force,
                    0.25f)
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
            this.PostNotification(NotificationType.DidConnectRoar);
            target.GetUnitComponent().HealthComponent.DamageOwner(Damage, this, Owner);
        }
    }
}