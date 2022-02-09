using System.Collections.Generic;
using System.Linq;
using Abilities;
using TMPro;
using UnityEngine;

namespace UI.InGameShop.AbilitiesScreen {
    public class SkillScrollViewPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private SkillScrollViewToggle togglePrefab;
        [SerializeField] private GameObject toggleSelections;
        private List<SkillScrollViewToggle> _toggles;
        public Ability associatedAbility { get; private set; }

        private void OnEnable() {
            _toggles = new List<SkillScrollViewToggle>();
        }

        private void OnDisable() {
            foreach (var toggle in _toggles) {
                Destroy(toggle);
            }
            _toggles = null;
        }

        public void InspectAbility() {
            if (associatedAbility == null) return;
            _toggles[0]._toggle.isOn = true;
        }

        public void UpdateSkillScrollViewPanel(Ability ability) {
            foreach (var toggle in _toggles) {
                Destroy(toggle);
            }
            _toggles.Clear();
            associatedAbility = ability;
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