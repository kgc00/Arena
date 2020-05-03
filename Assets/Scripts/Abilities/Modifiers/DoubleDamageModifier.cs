using UnityEngine;

namespace Abilities.Modifiers
{
    public class DoubleDamageModifier : AttackAbilityModifier
    {
        public DoubleDamageModifier(Ability ability) : base(ability) { }

        public override void Handle()
        {
            Ability.Damage *= 2;
            Debug.Log($"Calling {ToString()} to double damage.  Damage is now {Ability.Damage}");
            base.Handle();
        }
    }
}