﻿using System.Globalization;
using Components;
using Data.Types;
using TMPro;
using Units;
using UnityEngine;

namespace UI.InGameShop.StatScreen {
    public class StatPanel : MonoBehaviour {
        [SerializeField] public StatScreen StatScreen;
        [SerializeField] public StatType StatType;
        [SerializeField] private TextMeshProUGUI StatNameText;
        [SerializeField] private TextMeshProUGUI StatDescriptionText;
        [SerializeField] private TextMeshProUGUI StatValueText;
        private StatsComponent _statsComponent;
        private int _baseValue;
        private int _newValue;
        public bool _shouldUpdateTextInEditor;

        private void OnValidate() {
            if (_shouldUpdateTextInEditor) {
                UpdateText();
            }
        }

        private void OnEnable() {
            var purchasingUnit = InGameShopManager.Instance.PurchasingUnit;
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
                StatDescriptionText.text = Utils.StatHelpers.GetDescription(StatType, _newValue);
            }

            if (StatValueText != null && _statsComponent != null) {
                StatValueText.text = _newValue.ToString(CultureInfo.InvariantCulture);
            }
        }

        public void HandleDecrement() {
            // this.PostNotification(NotificationType.InsufficientFundsForPurchase);
            if (_baseValue > _newValue - 1) return;
            _newValue -= StatType == StatType.MovementSpeed ? 5 : 1;
            StatScreen.IncrementSkillBank();
            UpdateText();
        }

        public void HandleIncrement() {
            // this.PostNotification(NotificationType.InsufficientFundsForPurchase);
            if (StatScreen.SkillPointBank <= 0 || Utils.StatHelpers.CapForStat(StatType) <= _newValue) return;
            _newValue += StatType == StatType.MovementSpeed ? 5 : 1;
            StatScreen.DecrementSkillBank();
            UpdateText();
        }

        public void HandlePurchase(Unit purchasingUnit) {
            purchasingUnit.StatsComponent.Stats.StatFromEnum(StatType).Value = _newValue;
            _baseValue = _newValue;
            UpdateText();
        }
    }
}