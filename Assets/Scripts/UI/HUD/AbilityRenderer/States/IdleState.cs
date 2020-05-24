namespace UI.HUD {
    public class IdleState : State {
        public IdleState(AbilityRenderer AbilityRenderer) : base(AbilityRenderer) { }

        public override State HandleUpdate() {
            if (abilityRenderer.ability.Cooldown.IsOnCooldown)
                return new CooldownState(abilityRenderer);

            return null;
        }
    }
}