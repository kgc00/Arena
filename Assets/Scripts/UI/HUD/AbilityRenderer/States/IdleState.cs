using UnityEngine;

namespace UI.HUD.AbilityRenderer.States {
    public class IdleState : State {
        public IdleState(AbilityRenderer abilityRenderer) : base(abilityRenderer) { }

        public override State HandleUpdate() {
            return abilityRenderer.Ability.Cooldown.IsOnCooldown ? new CooldownState(abilityRenderer) : null;
        }
    }
}