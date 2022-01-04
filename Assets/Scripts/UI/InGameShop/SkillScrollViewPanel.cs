using System.Collections.Generic;
using System.Linq;
using Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameShop {
    public class SkillScrollViewPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private GameObject toggleSelections;
        private List<GameObject> toggles;

        private void OnEnable() {
            toggles = new List<GameObject>();
        }

        private void OnDisable() {
            foreach (var toggle in toggles) {
                Destroy(toggle);
            }
            toggles = null;
        }

        public void UpdateSkillScrollViewPanel(Ability ability) {
            foreach (var toggle in toggles) {
                Destroy(toggle);
            }
            toggles.Clear();
            
            skillName.SetText(ability.DisplayName);
            ability.EquipableModifiers.ForEach(equipableModifierType => {
                var toggle = Instantiate(togglePrefab, toggleSelections.transform);
                toggles.Add(toggle);
                toggle.GetComponent<Toggle>().interactable = !ability.Modifiers
                    .Select(equippedModifierType => equippedModifierType.Type)
                    .Contains(equipableModifierType);
            });
        }
    }
}