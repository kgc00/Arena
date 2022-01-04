using Abilities.Buffs;
using Data.AbilityData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameShop {
    public class SkillInspectorView : MonoBehaviour {
        public AbilityData AbilityData;
        
        [SerializeField] private Image _skillImage;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private TextMeshProUGUI _areaOfEffectText;
        [SerializeField] private TextMeshProUGUI _durationText;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _damageText;
        private void OnEnable() {
            UpdateAbilityInspectorShopData(AbilityData);
        }

        void UpdateAbilityInspectorShopData(AbilityData abilityData) {
            AbilityData = abilityData;
            _skillImage.sprite = AbilityData.icon;
            _titleText.SetText(AbilityData.displayName);
            _descriptionText.SetText(AbilityData.description);

            _durationText.SetText(AbilityData.duration.ToString());
            _cooldownText.SetText(AbilityData.cooldown.ToString());
            _areaOfEffectText.SetText(AbilityData.areaOfEffectRadius.ToString());

            if (AbilityData is AttackAbilityData attackAbilityData) {
                _damageText.SetText(attackAbilityData.Damage.ToString());
            }
        }
    }
}