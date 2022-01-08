using System.Collections.Generic;
using System.Linq;
using Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameShop {
    public class SkillScrollViewPanel : MonoBehaviour {
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

        public void UpdateSkillScrollViewPanel(Ability ability) {
            foreach (var toggle in _toggles) {
                Destroy(toggle);
            }
            _toggles.Clear();
            
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