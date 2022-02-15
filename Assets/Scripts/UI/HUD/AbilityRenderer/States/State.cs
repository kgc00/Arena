namespace UI.HUD {
    public abstract class State {
        protected AbilityRenderer.AbilityRenderer abilityRenderer;
        public virtual void Enter () { }
        public virtual void Exit () { }
        public virtual State HandleUpdate () { return null; }

        public State(AbilityRenderer.AbilityRenderer AbilityRenderer) {
            abilityRenderer = AbilityRenderer;
        }
    }
}