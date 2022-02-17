using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.AbilityRenderer.States {
    public class LockedState : State {
        private CanvasGroup _overallCanvasGroup;
        private int _verticalOffset;

        public LockedState(AbilityRenderer abilityRenderer) : base(abilityRenderer) {
            _verticalOffset = abilityRenderer.cooldownVerticalOffset;
            _overallCanvasGroup = abilityRenderer._overallCanvasGroup;
        }

        public override State HandleUpdate() {
            if (abilityRenderer.ability.Unlocked)
                return new IdleState(abilityRenderer);
            else
                return null;
        }

        public override void Enter() {
            abilityRenderer.VerticalLayoutGroup.padding.top = _verticalOffset;
            _overallCanvasGroup.alpha = 0.5f;
        }

        public override void Exit() {
            abilityRenderer.VerticalLayoutGroup.padding.top = 0;
            _overallCanvasGroup.alpha = 1f;
            LayoutRebuilder.ForceRebuildLayoutImmediate(abilityRenderer._RectTransform);
        }
    }
}