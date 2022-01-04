using Abilities.Modifiers.AbilityModifierShopData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private void OnEnable() {
            UpdateAbilityModifierShopData(AbilityModifierShopData);
        }

        void UpdateAbilityModifierShopData(AbilityModifierShopData abilityModifierShopData) {
            AbilityModifierShopData = abilityModifierShopData;
            _skillModifierImage.sprite = AbilityModifierShopData.Image;
            _titleText.SetText(AbilityModifierShopData.Title);
            _costText.SetText(AbilityModifierShopData.Cost.ToString());
            _descriptionText.SetText(AbilityModifierShopData.Description);

            var player = InGameShopManager.Instance.Player;
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

        }
    }
}