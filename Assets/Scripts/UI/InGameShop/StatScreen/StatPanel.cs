using System.Globalization;
using Components;
using Data.Types;
using TMPro;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace UI.InGameShop.StatScreen {
    public class StatPanel : MonoBehaviour {
        [SerializeField] public StatScreen StatScreen;
        [SerializeField] public StatType StatType;
        [SerializeField] private TextMeshProUGUI StatNameText;
        [SerializeField] private TextMeshProUGUI StatDescriptionText;
        [SerializeField] private TextMeshProUGUI StatValueText;
        [SerializeField] private GameObject incrementButton;
        [SerializeField] private GameObject decrementButton;
        private StatsComponent _statsComponent;
        private int _baseValue;
        private int _newValue;
        public bool _shouldUpdateTextInEditor;
        private InGameShopManager _inGameShopManager;

        private void OnValidate() {
            if (_shouldUpdateTextInEditor) {
                UpdateText();
            }
        }

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            var purchasingUnit = _inGameShopManager.PurchasingUnit;
            _statsComponent = purchasingUnit ? purchasingUnit.StatsComponent : null;
            if (_statsComponent != null) {
                _baseValue = _statsComponent.StatFromEnum(StatType).Value;
                _newValue = _baseValue;
            }

            UpdateText();
        }

        private void UpdateText() {
            if (StatNameText != null) {
                StatNameText.text = StatType.ToString();
            }

            if (StatDescriptionText != null) {
                StatDescriptionText.text = StatHelpers.GetDescription(StatType, _newValue);
            }

            if (StatValueText != null && _statsComponent != null) {
                StatValueText.text = _newValue.ToString(CultureInfo.InvariantCulture);
            }

            if ( _statsComponent.StatFromEnum(StatType).Value >= StatHelpers.CapForStat(StatType)) {
                incrementButton.SetActive(false);
                decrementButton.SetActive(false);
            }
            else {
                incrementButton.SetActive(true);
                decrementButton.SetActive(true);
            }
        }

        public void HandleDecrement() {
            // this.PostNotification(NotificationType.InsufficientFundsForPurchase);
            if (_baseValue > _newValue - 1) {
                this.PostNotification(NotificationType.UISoftWarning);
                return;
            }
            _newValue -= StatType == StatType.MovementSpeed ? 5 : 1;
            StatScreen.IncrementSkillBank();
            UpdateText();
            this.PostNotification(NotificationType.ClickDecrement);
        }

        public void HandleIncrement() {
            // this.PostNotification(NotificationType.InsufficientFundsForPurchase);
            if (StatScreen.SkillPointBank <= 0 || StatHelpers.CapForStat(StatType) <= _newValue) {
                this.PostNotification(NotificationType.UISoftWarning);
                return;
            }
            _newValue += StatType == StatType.MovementSpeed ? 5 : 1;
            StatScreen.DecrementSkillBank();
            UpdateText();
            this.PostNotification(NotificationType.ClickIncrement);
        }

        public void HandlePurchase(Unit purchasingUnit) {
            purchasingUnit.StatsComponent.Stats.StatFromEnum(StatType).Value = _newValue;
            _baseValue = _newValue;
            UpdateText();
        }
    }
}