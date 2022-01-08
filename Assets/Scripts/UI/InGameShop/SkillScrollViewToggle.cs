using System;
using System.Collections.Generic;
using Abilities;
using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;
using Data.Modifiers;
using Data.Types;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop {
    public class SkillScrollViewToggle : MonoBehaviour {
        public AbilityData AbilityModel { get; private set; }
        public AbilityModifierShopData ModifierShopData { get; private set; }
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Image _background;
        [SerializeField] private Image _frame;
        private bool _initialized;
        public bool IsPurchased;

        private void OnEnable() {
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void OnDisable() {
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        public void HandleToggle(bool toggleValue) {
            if (toggleValue) {
                this.PostNotification(NotificationType.SkillScrollViewToggleToggledOn,
                    new SkillScrollViewToggleEvent(AbilityModel, ModifierShopData, IsPurchased));
                _frame.enabled = true;
            }
            else {
                _frame.enabled = false;
            }
        }

        public SkillScrollViewToggle Initialize(AbilityData abilityModel,
            AbilityModifierType modifierShopDataType, bool isPurchased) {
            AbilityModel = abilityModel;
            ModifierShopData = Utils.AbilityFactory.AbilityModifierShopDataFromType(modifierShopDataType);
            _background.sprite = ModifierShopData.Image;
            Debug.Assert(SkillScrollView.ToggleGroup != null);
            _toggle.group = SkillScrollView.ToggleGroup;
            IsPurchased = isPurchased;
            _initialized = true;
            return this;
        }

        public void HandlePurchase(object sender, object args) {
            if (!_initialized || !_toggle.isOn) return;

            // _toggle.interactable = false;
            IsPurchased = true;
            _toggle.isOn = false;
        }
    }
}