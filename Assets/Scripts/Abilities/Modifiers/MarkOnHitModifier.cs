using Stats;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.Modifiers
{
    public class MarkOnHitModifier : AttackAbilityModifier
    {
        public MarkOnHitModifier(Ability ability) : base(ability) { }

        public override void Handle()
        {
            Debug.Log($"Calling {ToString()} to add a mark on collision.");
            Ability.onAbilityConnection.Insert(0, AddMark);
            base.Handle();
        }
        
        private void AddMark(GameObject target, GameObject projectile = null)
        {
            StatusHelper.AddMark(target);
        }
    }
}