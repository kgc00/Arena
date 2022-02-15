using UnityEngine;

namespace UI.HUD.AbilityRenderer.States {
    public class LockedState : State {
        private CanvasGroup _overallCanvasGroup;
        private int _verticalOffset;

        public LockedState(AbilityRenderer abilityRenderer) : base(abilityRenderer) {
            _verticalOffset = abilityRenderer.cooldownVerticalOffset;
            _overallCanvasGroup = abilityRenderer._overallCanvasGroup;
        }

        public override State HandleUpdate() {
            return abilityRenderer.ability.Unlocked ? new IdleState(abilityRenderer) : null;
        }

        public override void Enter() {
            base.Enter();
            abilityRenderer.VerticalLayoutGroup.padding.top = _verticalOffset;
            _overallCanvasGroup.alpha = 0.5f;
        }

        public override void Exit() {
            base.Exit();
            abilityRenderer.VerticalLayoutGroup.padding.top = 0;
            _overallCanvasGroup.alpha = 1f;
        }
    }
}