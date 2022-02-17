using Data.AbilityData;
using Data.Types;
using TMPro;
using UI.InGameShop.AbilitiesScreen.SkillScrollView;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen.AbilityInspector {
    public class SkillInspectorView : MonoBehaviour {
        public AbilityData AbilityData;

        [SerializeField] private Image _skillImage;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private TextMeshProUGUI _areaOfEffectText;
        [SerializeField] private TextMeshProUGUI _durationText;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _damageText;

        [SerializeField] private GameObject _enhancesSkillText;        
        [SerializeField] private Material canPuchaseCostDifferenceMaterial;
        [SerializeField] private Material cannotPuchaseCostDifferenceMaterial;
        [SerializeField] private GameObject _costView;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _costDifferenceText;
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            
            this.AddObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.AddObserver(HandleLockedSkillInspected, NotificationType.LockedSkillInspected);
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }
        

        private void OnDisable() {
            this.RemoveObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.RemoveObserver(HandleLockedSkillInspected, NotificationType.LockedSkillInspected);
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void HandlePurchase(object sender, object args) {
            // GO should only be active while purchasing this ability or modifier
            // no need to filter event args
            UpdateCostText(AbilityData.unlocked);
        }

        private void HandleLockedSkillInspected(object sender, object args) {
            if (args is LockedSkillInspectedEvent lockedSkillInspectedEvent) {
                _enhancesSkillText.SetActive(false);
                UpdateAbilityInspectorShopData(lockedSkillInspectedEvent.Model);
                UpdateCostText(lockedSkillInspectedEvent.Model.unlocked);
            }
        }

        void HandleSkillScrollViewToggleToggledOn(object sender, object args) {
            if (!(args is SkillScrollViewToggleEvent toggleEvent)) return;

            _enhancesSkillText.SetActive(true);
            UpdateAbilityInspectorShopData(toggleEvent.AbilityModel);
            UpdateCostText(true);
        }

        void UpdateAbilityInspectorShopData(AbilityData abilityData) {
            AbilityData = abilityData;
            _skillImage.sprite = AbilityData.icon;
            _skillImage.enabled = true;
            _titleText.SetText(AbilityData.displayName);
            _descriptionText.SetText(AbilityData.description);

            _durationText.SetText(AbilityData.duration.ToString());
            _cooldownText.SetText(AbilityData.cooldown.ToString());
            _areaOfEffectText.SetText(AbilityData.areaOfEffectRadius.ToString());

            if (AbilityData is AttackAbilityData attackAbilityData) {
                _damageText.SetText(attackAbilityData.Damage.ToString());
            }
        }
        
        private void UpdateCostText(bool isPurchased) {
            if (!isPurchased) {
                var player = _inGameShopManager.PurchasingUnit;
                if (player == null) return;
                var operation = player.FundsComponent.ContainsEnoughFunds(AbilityData.unlockCost);
                if (operation.containsEnoughFunds) {
                    _costDifferenceText.fontMaterial = canPuchaseCostDifferenceMaterial;
                    _costDifferenceText.SetText($"(+{operation.remainder})");
                }
                else {
                    _costDifferenceText.fontMaterial = cannotPuchaseCostDifferenceMaterial;
                    _costDifferenceText.SetText($"(-{operation.remainder})");
                }
                _costText.SetText(AbilityData.unlockCost.ToString());
                _costView.SetActive(true);
            }
            else {
                _costView.SetActive(false);
            }
        }
    }
}