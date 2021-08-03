using Data.Modifiers;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.Modifiers
{
    public class PersistentMarkOnHitModifier : AttackAbilityModifier
    {
        public PersistentMarkOnHitModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.PersistentAddMarkOnHit;
        }
        public override bool ShouldConsume() => false;
        public override void Handle()
        {
            Debug.Log($"Calling {ToString()} to add a mark on collision.");
            Ability.OnAbilityConnection.Insert(0, AddMark);
            base.Handle();
        }
        
        private void AddMark(GameObject target, GameObject projectile = null)
        {
            StatusHelper.AddMark(target);
        }
    }
}