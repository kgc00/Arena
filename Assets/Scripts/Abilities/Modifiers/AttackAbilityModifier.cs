namespace Abilities.Modifiers
{
    public class AttackAbilityModifier : AbilityModifier
    {
        // Hides parent's Ability ability.
        protected new AttackAbility Ability;

        public AttackAbilityModifier(Ability ability) : base(ability)
        {
            this.Ability = (AttackAbility) ability;
        }

        public override AbilityModifier InitializeModifier(Ability ability)
        {
            base.InitializeModifier(ability);
            Ability = (AttackAbility) ability;
            return this;
        }

        public override bool ShouldConsume() => true;
    }
}