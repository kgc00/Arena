using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Common;
using Components;
using Data.Items;
using Data.Params;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Status {
    public class Marked : MonoStatus {
        private Material _fresnel;
        private AttackAbility _mostRecentCatalyst;
        public override StatusType Type { get; protected set; } = StatusType.Marked;

        public override MonoStatus Initialize(Unit owner, bool isTimed, int amount) {
            _fresnel = MonoHelper.LoadMaterial(MaterialType.MarkOutline);
            return base.Initialize(owner, isTimed, amount);
        }

        public override MonoStatus Initialize(Unit owner, float duration, int amount) {
            _fresnel = MonoHelper.LoadMaterial(MaterialType.MarkOutline);
            return base.Initialize(owner, duration, amount);
        }

        protected override void EnableEffect() {
            var spawnPos = Owner.transform.position;
            spawnPos.y += 1.5f;
            var vfx = MonoHelper.SpawnVfx(VfxType.Mark, spawnPos);
            vfx.transform.SetParent(Owner.transform);
            var rend = Owner.transform.root.GetComponentInChildren<Renderer>();
            var materials = rend.materials.ToList();
            materials.Add(_fresnel);
            rend.materials = materials.ToArray();
        }

        public override void DisableEffect() {
            var rend = Owner.transform.root.GetComponentInChildren<Renderer>();
            // will be null when unit dies to triggering the marked effect
            if (rend != null) {
                var materials = rend.materials;
                var withoutFresnel = materials.ToList().Where(m => m.shader.name != _fresnel.shader.name).ToArray();
                rend.materials = withoutFresnel;
            }

            base.DisableEffect();
        }

        public override void TriggerEffect(Ability catalyst) {
            if (catalyst is AttackAbility attackAbility) {
                var hasExplosionUpgrade = catalyst.Owner.PurchasedItems.Contains(ItemType.ExplosiveMark);
                var totalDamage = attackAbility.Damage + Amount;
                Owner.HealthComponent.DamageOwner(totalDamage);
                if (Owner.HealthComponent.CurrentHp - totalDamage <= 0 && hasExplosionUpgrade) {
                    _mostRecentCatalyst = attackAbility;
                    var colliderParams = new SphereParams(3);
                    var _ = new GameObject("Mark Explosive Force")
                        .AddComponent<AoEComponent>()
                        .Initialize(colliderParams,
                            transform.position,
                            transform.position,
                            HandleEnterStrategy,
                            null,
                            null,
                            new List<ControlType> {ControlType.Ai},
                            default,
                            0.1f)
                        .gameObject;
                    MonoHelper.SpawnVfx(VfxType.MarkExplosion, Owner.transform.position);
                }
                else {
                    MonoHelper.SpawnVfx(VfxType.MarkTriggered, Owner.transform.position);
                }
            }

            base.TriggerEffect(catalyst);
        }

        private IEnumerator HandleEnterStrategy(Collider arg1, Rigidbody arg2, float arg3, Transform arg4) {
            var unit = arg1.gameObject.GetUnitComponent();
            if (unit == null) yield break;
            if (unit.StatusComponent.IsMarked()) {
                unit.StatusComponent.TriggerStatus(StatusType.Marked, _mostRecentCatalyst);
            }
            else {
                unit.HealthComponent.DamageOwner(Amount, _mostRecentCatalyst, _mostRecentCatalyst.Owner);
            }
        }

        public override void ReapplyStatus(int amount) {
            var spawnPos = Owner.transform.position;
            spawnPos.y += 1.5f;
            var vfx = MonoHelper.SpawnVfx(VfxType.Mark, spawnPos);
            vfx.transform.SetParent(Owner.transform);
            base.ReapplyStatus(amount);
        }
    }
}