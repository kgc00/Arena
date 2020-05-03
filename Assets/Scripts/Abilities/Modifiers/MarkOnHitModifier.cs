using UnityEngine;

namespace Abilities.Modifiers
{
    public class MarkOnHitModifier : AttackAbilityModifier
    {
        public MarkOnHitModifier(Ability ability) : base(ability) { }

        public override void Handle()
        {
            Debug.Log($"Calling {ToString()}");
            base.Handle();
        }
    }
}