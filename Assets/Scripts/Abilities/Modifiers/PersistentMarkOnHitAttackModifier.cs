using Data.Modifiers;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.Modifiers
{
    public class PersistentMarkOnHitAttackModifier : AttackAbilityModifier
    {
        public PersistentMarkOnHitAttackModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.PersistentAddMarkOnHit;
        }
        public override bool ShouldConsume() => false;
        public override void Handle()
        {
            Ability.OnAbilityConnection.Insert(0, AddMark);
            base.Handle();
        }
        
        private void AddMark(GameObject target, GameObject projectile = null) {
            if (target == null) return;
            var unit = target.GetUnitComponent();
            if (unit == null) return;
            unit.StatusComponent.AddStatus(StatusType.Marked, 1);
        }
    }
}