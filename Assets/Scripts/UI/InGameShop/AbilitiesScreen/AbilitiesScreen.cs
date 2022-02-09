using System.Linq;
using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;
using Data.Types;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen {
    public class AbilitiesScreen : ShopScreen {
        private AbilityData _selectedAbilityData;
        private AbilityModifierShopData _selectedModifierData;
        [SerializeField] private GameObject SkillInspectorWrapper;
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
            ToggleRenderInformation(true);
        }

        public void HandlePurchase() {
            var purchasingUnit = InGameShopManager.Instance.PurchasingUnit;
            Debug.Assert(purchasingUnit != null);
            var (containsEnoughFunds, remainder) =
                purchasingUnit.FundsComponent.ContainsEnoughFunds(_selectedModifierData.Cost);
            if (!containsEnoughFunds) {
                this.PostNotification(NotificationType.InsufficientFundsForPurchase);
                return;
            }

            purchasingUnit.FundsComponent.SetBalance(remainder);
            purchasingUnit
                .AbilityComponent
                .equippedAbilitiesByButton
                .Values
                .First(x => x.Type == _selectedAbilityData.type)
                .AddModifier(_selectedModifierData.Type);
            this.PostNotification(NotificationType.PurchaseComplete);
        }

        private void ToggleRenderInformation(bool visibility) {
            for (int i = 0; i < SkillInspectorWrapper.transform.childCount; i++) {
                for (int j = 0; j < SkillInspectorWrapper.transform.GetChild(i).childCount; j++) {
                    SkillInspectorWrapper.transform.GetChild(i).GetChild(j)
                        .gameObject.SetActive(visibility);
                }
            }
        }
    }
}