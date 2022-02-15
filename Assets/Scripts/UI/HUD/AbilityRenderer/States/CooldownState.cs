using System;
using System.Globalization;
using UnityEngine;

namespace UI.HUD {
    public class CooldownState :State {
        private readonly int _verticalOffset;
        private CanvasGroup _canvasGroup;

        public CooldownState(AbilityRenderer.AbilityRenderer abilityRenderer) : base(abilityRenderer) {
            _verticalOffset = abilityRenderer.cooldownVerticalOffset;
            _canvasGroup = abilityRenderer._canvasGroup;
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
            if (!abilityRenderer.ability.Cooldown.IsOnCooldown) 
                return new IdleState(abilityRenderer);
            
            abilityRenderer.timer.SetText(TimeLeft());
            abilityRenderer.iconRadialFill.fillAmount = FillAmount();
            return null;
        }

        private string TimeLeft() {
            var rounded = Mathf.Round(abilityRenderer.ability.Cooldown.TimeLeft * 10) / 10;
            return rounded.ToString(CultureInfo.InvariantCulture);
        }

        private float FillAmount() {
            var timeLeft = abilityRenderer.ability.Cooldown.TimeLeft;
            var cooldownTime = abilityRenderer.ability.Cooldown.CooldownTime;
            var fill =  1 - timeLeft / cooldownTime;
            return fill;
        }
    }
}