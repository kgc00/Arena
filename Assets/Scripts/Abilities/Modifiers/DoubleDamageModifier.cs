using Data.Modifiers;
using UnityEngine;

namespace Abilities.Modifiers
{
    public class DoubleDamageModifier : AttackAbilityModifier
    {
        public DoubleDamageModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.DoubleDamage;
        }

        public override void Handle()
        {
            Ability.Damage *= 2;
            base.Handle();
        }

        public override bool ShouldConsume() => false;
    }
}