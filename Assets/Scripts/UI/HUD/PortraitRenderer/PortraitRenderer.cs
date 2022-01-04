using System.Globalization;
using Components;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.PortraitRenderer {
    public class PortraitRenderer : MonoBehaviour {
        private Unit _unit;
        [SerializeField]private GameObject nameGo;
        [SerializeField]private GameObject portraitGo;
        private Image _portrait;
        private TextMeshProUGUI _portraitName;
        
        [SerializeField]private GameObject healthFillGo;
        [SerializeField]private GameObject healthTextGo;
        private Image _healthFill;
        private TextMeshProUGUI _healthText;
        public PortraitRenderer Initialize(Unit unit) {
            _unit = unit;
            _portraitName = nameGo.GetComponent<TextMeshProUGUI>();
            _portrait = portraitGo.GetComponent<Image>();
            _healthFill = healthFillGo.GetComponent<Image>();
            _healthText = healthTextGo.GetComponent<TextMeshProUGUI>();
            
            _portraitName.SetText(unit.name);
            _portrait.sprite = unit.Portrait;
            UpdateHealthValue();

            HealthComponent.OnHealthChanged += UpdateHealthValue;
            
            return this;
        }

        private void UpdateHealthValue(Unit u, float arg2) {
            if (u == _unit)
                UpdateHealthValue();
        }

        void UpdateHealthValue() {
            _healthFill.fillAmount = _unit.HealthComponent.CurrentHp / _unit.HealthComponent.MaxHp;
            _healthText.SetText(_unit.HealthComponent.CurrentHp.ToString(CultureInfo.InvariantCulture));
        }
    }
}