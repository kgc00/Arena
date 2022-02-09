using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameShop.AbilitiesScreen {
    public class SkillScrollView : MonoBehaviour {
        [SerializeField] private Transform gridTransform;
        [SerializeField] private SkillScrollViewPanel SkillScrollViewPanelPrefab;
        [SerializeField] private ToggleGroup _toggleGroup;
        public static ToggleGroup ToggleGroup;
        [SerializeField] private List<SkillScrollViewPanel> SkillScrollViewPanels;
        [SerializeField] private ScrollRect _scrollRect;
        private void OnEnable() {
            SkillScrollViewPanels = new List<SkillScrollViewPanel>();
            ToggleGroup = _toggleGroup;
            UpdateSkillScrollView();
            StartCoroutine(DelayInspectToggle());
        }

        private IEnumerator DelayInspectToggle() {
            yield return new WaitForEndOfFrame();
            SkillScrollViewPanels[0].InspectAbility();
        }

        private void OnDisable() {
            foreach (var panel in SkillScrollViewPanels) {
                Destroy(panel.gameObject);
            }
            SkillScrollViewPanels.Clear();
        }
        
        void UpdateSkillScrollView() {
            var player = InGameShopManager.Instance.PurchasingUnit;
            if (player == null) return;

            foreach (var panel in SkillScrollViewPanels) {
                Destroy(panel.gameObject);
            }
            SkillScrollViewPanels.Clear();
            
            var abilities = player.AbilityComponent.equippedAbilitiesByButton.Select(x => x.Value).ToList();
            foreach (var ability in abilities) {
                var panel = Instantiate(SkillScrollViewPanelPrefab, gridTransform);
                panel.UpdateSkillScrollViewPanel(ability);
                SkillScrollViewPanels.Add(panel);
            }
        }
    }
}