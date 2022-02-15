namespace UI.HUD.AbilityRenderer.States {
    public abstract class State {
        protected HUD.AbilityRenderer.AbilityRenderer abilityRenderer;
        public virtual void Enter () { }
        public virtual void Exit () { }
        public virtual State HandleUpdate () { return null; }

        public State(HUD.AbilityRenderer.AbilityRenderer AbilityRenderer) {
            abilityRenderer = AbilityRenderer;
        }
    }
}