using System.Collections.Generic;
using System.Linq;
using Abilities;
using TMPro;
using UnityEngine;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public class UnlockedSkillScrollViewPanel : SkillScrollViewPanel {
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private SkillScrollViewToggle togglePrefab;
        [SerializeField] private GameObject toggleSelections;
        private List<SkillScrollViewToggle> _toggles;

        private void OnEnable() {
            _toggles = new List<SkillScrollViewToggle>();
        }

        private void OnDisable() {
            foreach (var toggle in _toggles) {
                Destroy(toggle);
            }
            _toggles = null;
        }

        public override void InspectAbility(bool isSilent = false) {
            if (AssociatedAbility == null) return;
            if (isSilent) {
                _toggles[0].SilenceNextToggle();
            }
            _toggles[0]._toggle.isOn = true;
        }

        public override void UpdateSkillScrollViewPanel(Ability ability) {
            foreach (var toggle in _toggles) {
                Destroy(toggle);
            }
            _toggles.Clear();
            AssociatedAbility = ability;
            skillName.SetText(ability.DisplayName);
            ability.EquipableModifiers.ForEach(equipableModifierType => {
                var isPurchased = ability.Modifiers
                    .Select(equippedModifierType => equippedModifierType.Type)
                    .Contains(equipableModifierType);
                var toggle = Instantiate(togglePrefab, toggleSelections.transform).Initialize(ability.Model, equipableModifierType, isPurchased);
                _toggles.Add(toggle);
            });
        }
    }
}