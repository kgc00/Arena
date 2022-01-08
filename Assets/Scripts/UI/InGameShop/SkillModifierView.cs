using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;
using Data.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop {
    public class SkillModifierView : MonoBehaviour {
        public AbilityModifierShopData AbilityModifierShopData;

        [SerializeField] private Material canPuchaseCostDifferenceMaterial;
        [SerializeField] private Material cannotPuchaseCostDifferenceMaterial;
        [SerializeField] private Image _skillModifierImage;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _costDifferenceText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private GameObject _canPurchaseText;
        [SerializeField] private GameObject _hasPurchasedText;
        private void OnEnable() {
            this.AddObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void OnDisable() {
            this.RemoveObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void HandleSkillScrollViewToggleToggledOn(object sender, object args) {
            if (!(args is SkillScrollViewToggleEvent toggleEvent)) return;

            UpdateAbilityModifierShopData(toggleEvent.AbilityModifierShopData, toggleEvent.IsPurchased);
        }

        void UpdateAbilityModifierShopData(AbilityModifierShopData abilityModifierShopData, bool isPurchased) {
            AbilityModifierShopData = abilityModifierShopData;
            _skillModifierImage.sprite = AbilityModifierShopData.Image;
            _titleText.SetText(AbilityModifierShopData.Title);
            _costText.SetText(AbilityModifierShopData.Cost.ToString());
            _descriptionText.SetText(AbilityModifierShopData.Description);
            UpdateCostText(isPurchased);
        }


        private void UpdateCostText(bool isPurchased) {
            if (!isPurchased) {
                var player = InGameShopManager.Instance.PurchasingUnit;
                if (player == null) return;
                var operation = player.FundsComponent.ContainsEnoughFunds(AbilityModifierShopData.Cost);
                if (operation.containsEnoughFunds) {
                    _costDifferenceText.fontMaterial = canPuchaseCostDifferenceMaterial;
                    _costDifferenceText.SetText($"(+{operation.remainder})");
                }
                else {
                    _costDifferenceText.fontMaterial = cannotPuchaseCostDifferenceMaterial;
                    _costDifferenceText.SetText($"(-{operation.remainder})");
                }
                _hasPurchasedText.SetActive(false);
                _canPurchaseText.SetActive(true);
            }
            else {
                _canPurchaseText.SetActive(false);
                _hasPurchasedText.SetActive(true);
            }
        }

        public void HandlePurchase(object sender, object args) {
            UpdateCostText(true);
        }
    }
}