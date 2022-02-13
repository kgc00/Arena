using Data.Modifiers;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.Modifiers {
    public class MarkOnHitModifier : AttackAbilityModifier {
        public MarkOnHitModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.AddMarkOnHit;
        }

        public override void Handle() {
            Ability.OnAbilityConnection.Insert(0, AddMark);
            base.Handle();
        }

        private void AddMark(GameObject target, GameObject projectile = null) {
            if (!target.TryGetComponent<Unit>(out var unit)) return;
            unit.StatusComponent.AddStatus(StatusType.Marked, 1);
        }
    }
}