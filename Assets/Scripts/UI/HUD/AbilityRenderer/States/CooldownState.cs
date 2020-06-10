using System;
using System.Globalization;
using UnityEngine;

namespace UI.HUD {
    public class CooldownState :State {
        public CooldownState(AbilityRenderer AbilityRenderer) : base(AbilityRenderer) { }

        public override void Enter() {
            abilityRenderer.icon.color = new Color(0.25f,0.25f,0.25f,1);
            abilityRenderer.iconRadialFill.fillAmount = 0;
            abilityRenderer.VerticalLayoutGroup.padding.bottom = -110;
            abilityRenderer.timer.SetText(TimeLeft());
        }

        public override void Exit() {
            abilityRenderer.icon.color = new Color(1,1,1,1);
            abilityRenderer.iconRadialFill.fillAmount = 1;
            abilityRenderer.VerticalLayoutGroup.padding.bottom = 0;
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
            var fill =  timeLeft / cooldownTime;
            return fill ?? 0;
        }
    }
}