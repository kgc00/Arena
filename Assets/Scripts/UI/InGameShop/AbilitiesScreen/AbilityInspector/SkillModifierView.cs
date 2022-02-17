using Abilities.Modifiers.AbilityModifierShopData;
using Data.Types;
using TMPro;
using UI.InGameShop.AbilitiesScreen.SkillScrollView;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen.AbilityInspector {
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
        private InGameShopManager _inGameShopManager;
        [SerializeField] private GameObject graphicalElements;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            this.AddObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
            this.AddObserver(HandleLockedSkillInspected, NotificationType.LockedSkillInspected);
        }

        private void HandleLockedSkillInspected(object sender, object args) {
            graphicalElements.SetActive(false);
        }

        private void OnDisable() {
            this.RemoveObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
            this.RemoveObserver(HandleLockedSkillInspected, NotificationType.LockedSkillInspected);
        }

        private void HandleSkillScrollViewToggleToggledOn(object sender, object args) {
            if (!(args is SkillScrollViewToggleEvent toggleEvent)) return;

            graphicalElements.SetActive(true);
            UpdateAbilityModifierShopData(toggleEvent.AbilityModifierShopData, toggleEvent.IsPurchased);
        }

        void UpdateAbilityModifierShopData(AbilityModifierShopData abilityModifierShopData, bool isPurchased) {
            AbilityModifierShopData = abilityModifierShopData;
            _skillModifierImage.sprite = AbilityModifierShopData.Image;
            _skillModifierImage.enabled = true;
            _titleText.SetText(AbilityModifierShopData.Title);
            _costText.SetText(AbilityModifierShopData.Cost.ToString());
            _descriptionText.SetText(AbilityModifierShopData.Description);
            UpdateCostText(isPurchased);
        }


        private void UpdateCostText(bool isPurchased) {
            if (!isPurchased) {
                var player = _inGameShopManager.PurchasingUnit;
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