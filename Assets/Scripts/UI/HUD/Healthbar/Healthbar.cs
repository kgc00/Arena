using System.Globalization;
using System.Linq;
using Components;
using Data.Types;
using Players;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.NotificationCenter;

namespace UI.HUD.Healthbar {
    public class Healthbar : MonoBehaviour {
        private Unit _unit;

        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image _healthFill;
        [SerializeField] private TextMeshProUGUI _healthText;

        private void OnEnable() {
            this.AddObserver(HandleDidSpawn, NotificationType.UnitDidSpawn);
            this.AddObserver(HandleStatUpdate, NotificationType.DidLevelUp);
            this.AddObserver(HandleStatUpdate, NotificationType.PurchaseComplete);
            HealthComponent.OnHealthChanged += UpdateHealthValue;
        }

        private void HandleDidSpawn(object arg1, object arg2) {
            if (_unit != null || !(arg2 is Unit u)) return;
            if (u.Owner.ControlType == ControlType.Local) {
                Initialize();
            }
        }

        private void OnDisable() {
            this.RemoveObserver(HandleDidSpawn, NotificationType.UnitDidSpawn);
            this.RemoveObserver(HandleStatUpdate, NotificationType.DidLevelUp);
            this.RemoveObserver(HandleStatUpdate, NotificationType.PurchaseComplete);
            HealthComponent.OnHealthChanged -= UpdateHealthValue;
        }

        private void Start() {
            Initialize();
        }

        public void Initialize() {
            EnsureUnitAssigned();

            if (_unit == null) {
                return;
            }

            UpdateHealthValue();    
        }

        private void EnsureUnitAssigned() {
            if (_unit == null) {
                var playerTransform = Locator.GetClosestPlayerUnit(Vector3.zero);
                _unit = playerTransform ? playerTransform.gameObject.GetUnitComponent() : null;
            }
        }

        private void HandleStatUpdate(object arg1, object arg2) {
            EnsureUnitAssigned();

            if (_unit == null) return;
            
            levelText.SetText($"Level - {_unit.ExperienceComponent.Level}");
            UpdateHealthValue();
        }

        private void UpdateHealthValue(Unit u, float arg2) {
            EnsureUnitAssigned();

            if (u == _unit)
                UpdateHealthValue();
        }

        void UpdateHealthValue() {
            _healthFill.fillAmount = _unit.HealthComponent.CurrentHp / _unit.HealthComponent.MaxHp;
            _healthText.SetText(
                $"{Mathf.RoundToInt(_unit.HealthComponent.CurrentHp)}/{Mathf.RoundToInt(_unit.HealthComponent.MaxHp)}");
        }
    }
}