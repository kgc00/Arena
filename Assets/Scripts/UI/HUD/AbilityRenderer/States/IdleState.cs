namespace UI.HUD.AbilityRenderer.States {
    public class IdleState : State {
        public IdleState(HUD.AbilityRenderer.AbilityRenderer AbilityRenderer) : base(AbilityRenderer) { }

        public override State HandleUpdate() {
            if (abilityRenderer.ability.Cooldown.IsOnCooldown)
                return new CooldownState(abilityRenderer);

            return null;
        }
    }
}