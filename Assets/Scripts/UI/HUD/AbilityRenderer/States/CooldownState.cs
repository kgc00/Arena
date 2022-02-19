using System.Globalization;
using UnityEngine;

namespace UI.HUD.AbilityRenderer.States {
    public class CooldownState :State {
        private readonly int _verticalOffset;
        private CanvasGroup _canvasGroup;

        public CooldownState(HUD.AbilityRenderer.AbilityRenderer abilityRenderer) : base(abilityRenderer) {
            _verticalOffset = abilityRenderer.cooldownVerticalOffset;
            _canvasGroup = abilityRenderer._keyAndIconCanvasGroup;
        }

        public override void Enter() {
            _canvasGroup.alpha = 0.5f;
            abilityRenderer.icon.color = new Color(0.25f,0.25f,0.25f,1);
            abilityRenderer.iconRadialFill.fillAmount = 0;
            abilityRenderer.VerticalLayoutGroup.padding.top = _verticalOffset;
            abilityRenderer.timer.SetText(TimeLeft());
            
        }

        public override void Exit() {
            _canvasGroup.alpha = 1;
            abilityRenderer.icon.color = new Color(1,1,1,1);
            abilityRenderer.iconRadialFill.fillAmount = 1;
            abilityRenderer.VerticalLayoutGroup.padding.top = 0;
            abilityRenderer.timer.SetText("");
        }

        public override State HandleUpdate() {
            if (!abilityRenderer.Ability.Cooldown.IsOnCooldown) 
                return new IdleState(abilityRenderer);
            
            abilityRenderer.timer.SetText(TimeLeft());
            abilityRenderer.iconRadialFill.fillAmount = FillAmount();
            return null;
        }

        private string TimeLeft() {
            var rounded = Mathf.Round(abilityRenderer.Ability.Cooldown.TimeLeft * 10) / 10;
            return rounded.ToString(CultureInfo.InvariantCulture);
        }

        private float FillAmount() {
            var timeLeft = abilityRenderer.Ability.Cooldown.TimeLeft;
            var cooldownTime = abilityRenderer.Ability.Cooldown.CooldownTime;
            var fill =  1 - timeLeft / cooldownTime;
            return fill;
        }
    }
}