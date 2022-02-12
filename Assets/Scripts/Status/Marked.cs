using System.Linq;
using Abilities;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Status {
    public class Marked : MonoStatus {
        private Material _fresnel;
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
            var materials = rend.materials;
            var withoutFresnel = materials.ToList().Where(m => m.shader.name != _fresnel.shader.name).ToArray();
            rend.materials = withoutFresnel;
            base.DisableEffect();
        }

        public override void TriggerEffect(Ability catalyst) {
            if (catalyst is AttackAbility attackAbility) {
                Owner.HealthComponent.DamageOwner(attackAbility.Damage + Amount);
            }
            MonoHelper.SpawnVfx(VfxType.MarkExplosion, Owner.transform.position);
            base.TriggerEffect(catalyst);
        }

        public override void ReapplyStatus(int amount) {
            var vfx = MonoHelper.SpawnVfx(VfxType.Mark, Owner.transform.position);
            vfx.transform.SetParent(Owner.transform);
            base.ReapplyStatus(amount);
        }
    }
}