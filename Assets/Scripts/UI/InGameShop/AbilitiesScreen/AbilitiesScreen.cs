using System.Linq;
using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;
using Data.Types;
using UI.InGameShop.AbilitiesScreen.SkillScrollView;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen {
    public class AbilitiesScreen : ShopScreen {
        private AbilityData _selectedAbilityData;
        private AbilityModifierShopData _selectedModifierData;
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }

            this.AddObserver(HandleSkillScrollViewEvent, NotificationType.SkillScrollViewToggleToggledOn);
            this.AddObserver(HandleSkillScrollViewEvent, NotificationType.LockedSkillInspected);
        }

        private void OnDisable() {
            this.RemoveObserver(HandleSkillScrollViewEvent, NotificationType.SkillScrollViewToggleToggledOn);
            this.RemoveObserver(HandleSkillScrollViewEvent, NotificationType.LockedSkillInspected);
        }

        private void HandleSkillScrollViewEvent(object sender, object args) {
            if (args is LockedSkillInspectedEvent lockedSkillInspectedEvent) {
                _selectedAbilityData = lockedSkillInspectedEvent.Model;
                _selectedModifierData = null;
                return;
            }

            if (!(args is SkillScrollViewToggleEvent toggleEvent)) return;
            _selectedAbilityData = toggleEvent.AbilityModel;
            _selectedModifierData = toggleEvent.AbilityModifierShopData;
        }

        public void HandlePurchase() {
            var purchasingUnit = _inGameShopManager.PurchasingUnit;
            var isModifierPurchase = _selectedModifierData != null;
            var price = isModifierPurchase ? _selectedModifierData.Cost : _selectedAbilityData.unlockCost;
            var (containsEnoughFunds, remainder) =
                purchasingUnit.FundsComponent.ContainsEnoughFunds(price);
            if (!containsEnoughFunds) {
                this.PostNotification(NotificationType.InsufficientFundsForPurchase);
                return;
            }

            purchasingUnit.FundsComponent.SetBalance(remainder);
            PurchaseEvent purchaseEvent;
            if (isModifierPurchase) {
                purchasingUnit
                    .AbilityComponent
                    .equippedAbilitiesByButton
                    .Values
                    .First(x => x.Type == _selectedAbilityData.type)
                    .AddModifier(_selectedModifierData.Type);
                purchaseEvent = new PurchaseEvent(_selectedModifierData.Cost, _selectedModifierData.Type.ToString(), PurchaseEvent.PurchaseType.Modifier);
            }
            else {
                var ability = purchasingUnit.AbilityComponent
                    .equippedAbilitiesByButton
                    .Values
                    .First(x => x.Type == _selectedAbilityData.type);
                ability.Model.unlocked = true;
                ability.ResetInstanceValuesExcludingSpentModifiers();
                purchaseEvent =
                    new PurchaseEvent(_selectedAbilityData.unlockCost, _selectedAbilityData.type.ToString(), PurchaseEvent.PurchaseType.SkillUnlock);
            }

            this.PostNotification(NotificationType.PurchaseComplete, purchaseEvent);
        }
    }
}