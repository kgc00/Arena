using Abilities.Modifiers.AbilityModifierShopData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameShop {
    public class SkillModifierView : MonoBehaviour {
        public AbilityModifierShopData AbilityModifierShopData;

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
            _costDifferenceText.SetText(AbilityModifierShopData.Cost.ToString());
            _descriptionText.SetText(AbilityModifierShopData.Description);
        }
    }
}
