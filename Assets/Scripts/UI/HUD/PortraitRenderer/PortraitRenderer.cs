﻿using System.Globalization;
using Stats;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD {
    public class PortraitRenderer : MonoBehaviour {
        private Unit unit;
        private State state;
        [SerializeField]private GameObject nameGo;
        [SerializeField]private GameObject portraitGo;
        private Image portrait;
        private TextMeshProUGUI name;
        
        [SerializeField]private GameObject healthFillGo;
        [SerializeField]private GameObject healthTextGo;
        private Image healthFill;
        private TextMeshProUGUI healthText;
        public PortraitRenderer Initialize(Unit unit) {
            this.unit = unit;
            name = nameGo.GetComponent<TextMeshProUGUI>();
            portrait = portraitGo.GetComponent<Image>();
            healthFill = healthFillGo.GetComponent<Image>();
            healthText = healthTextGo.GetComponent<TextMeshProUGUI>();
            
            name.SetText(unit.name);
            portrait.sprite = unit.Portrait;
            UpdateHealthValue();

            HealthComponent.OnHealthChanged += UpdateHealthValue;
            
            return this;
        }

        private void UpdateHealthValue(Unit u, float arg2) {
            if (u == unit)
                UpdateHealthValue();
        }

        void UpdateHealthValue() {
            healthFill.fillAmount =  unit.HealthComponent.MaxHp / unit.HealthComponent.CurrentHp;
            healthText.SetText(unit.HealthComponent.CurrentHp.ToString(CultureInfo.InvariantCulture));
        }
    }
}