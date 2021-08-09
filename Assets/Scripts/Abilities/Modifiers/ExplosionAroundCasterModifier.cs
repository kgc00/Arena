using System.Collections;
using Common;
using Data.Modifiers;
using Data.Types;
using Projectiles;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.Modifiers {
    public class ExplosionAroundCasterModifier : BuffAbilityModifier {
        public ExplosionAroundCasterModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.ExplosionAroundCaster;
        }

        public override bool ShouldConsume() => false;

        public override void Handle() {
            Debug.Log($"Calling {ToString()} to add a mark on collision.");
            Ability.OnActivation.Insert(0, Explode);
            base.Handle();
        }

        private IEnumerator Explode(Vector3 cursorPosition) {
            var colliderParams = new SphereParams(Ability.Range / 2);
            var targetLocation = Ability.Owner.transform.position;

            // should create some list that I can iterate through-
            // foreach AoEEffect => gameobject.AddComponent<AoEComponent>().Initialize(AoEEffect);

            // Requires refactoring this logic out into a more generic model which would live on abilities themselves
            // May also need to have the AoEComponent inherit from Ability or something.
            var pGo = new GameObject("Push Force")
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    targetLocation,
                    targetLocation,
                    ForceStrategies.Strategies[ForceStrategyType.ForceAlongHeading],
                    null,
                    null,
                    Ability.AffectedFactions,
                    force: 185,
                    duration: Mathf.Max(Ability.Duration, 0.01f))
                .gameObject
                .AddComponent<AoEComponent>()
                .Initialize(colliderParams,
                    targetLocation,
                    targetLocation,
                    AddMarkAndDamage,
                    null,
                    null,
                    Ability.AffectedFactions,
                    force: default,
                    duration: Mathf.Max(Ability.Duration, 0.01f))
                .gameObject;
            // MonoHelper.SpawnVfx(VfxType.BurstImpact, targetLocation);
            yield break;
        }

        private IEnumerator AddMarkAndDamage(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            if (other.transform.root.TryGetComponent<Unit>(out var unit)) {
                if (unit == Ability.Owner) yield break;
                StatusHelper.AddMark(unit);
                unit.HealthComponent.DamageOwner(1);
            }
        }
    }
}