namespace Abilities.Modifiers
{
    public class BuffAbilityModifier : AbilityModifier
    {
        protected new BuffAbility Ability;

        public BuffAbilityModifier(Ability ability) : base(ability)
        {
            Ability = (BuffAbility) ability;
        }

        public override AbilityModifier InitializeModifier(Ability ability)
        {
            base.InitializeModifier(ability);
            Ability = (BuffAbility) ability;
            return this;
        }

        public override bool ShouldConsume() => Ability is BuffAbility ? true : false;
    }
}