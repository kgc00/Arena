﻿using System.Collections;
using Common;
using Components;
using Data.Modifiers;
using Data.Params;
using Data.Types;
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
                unit.StatusComponent.AddStatus(StatusType.Marked, 1);
                unit.HealthComponent.DamageOwner(1);
            }
        }
    }
}