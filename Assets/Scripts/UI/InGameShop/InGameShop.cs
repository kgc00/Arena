using System.Linq;
using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;
using Data.Types;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop {
    public class InGameShop : MonoBehaviour {
        private AbilityData _selectedAbilityData;
        private AbilityModifierShopData _selectedModifierData;
        private void OnEnable() {
            this.AddObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
        }

        private void OnDisable() {
            this.RemoveObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
        }

        private void HandleSkillScrollViewToggleToggledOn(object sender, object args) {
            if (!(args is SkillScrollViewToggleEvent toggleEvent)) return;
            _selectedAbilityData = toggleEvent.AbilityModel;
            _selectedModifierData = toggleEvent.AbilityModifierShopData;
        }

        public void HandlePurchase() {
            var purchasingUnit = InGameShopManager.Instance.PurchasingUnit;
            Debug.Assert(purchasingUnit != null);
            var (containsEnoughFunds, remainder) = purchasingUnit.FundsComponent.ContainsEnoughFunds(_selectedModifierData.Cost);
            if (!containsEnoughFunds) {
                this.PostNotification(NotificationType.InsufficientFundsForPurchase);
                return;
            }
            purchasingUnit.FundsComponent.SetBalance(remainder);
            purchasingUnit
                .AbilityComponent
                .equippedAbilities
                .Values
                .First(x => x.Type == _selectedAbilityData.type)
                .AddModifier(_selectedModifierData.Type);
            this.PostNotification(NotificationType.PurchaseComplete);
        }
    }
}