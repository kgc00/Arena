using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameShop {
    public class SkillScrollView : MonoBehaviour {
        [SerializeField] private Transform gridTransform;
        [SerializeField] private SkillScrollViewPanel SkillScrollViewPanelPrefab;
        [SerializeField] private ToggleGroup _toggleGroup;
        public static ToggleGroup ToggleGroup;
        [SerializeField] private List<SkillScrollViewPanel> SkillScrollViewPanels;

        private void OnEnable() {
            InGameShopManager.Instance.OnShopVisibilityToggled += HandleShopVisibilityToggled;
            SkillScrollViewPanels = new List<SkillScrollViewPanel>();
            ToggleGroup = _toggleGroup;
        }

        private void OnDisable() {
            InGameShopManager.Instance.OnShopVisibilityToggled -= HandleShopVisibilityToggled;
            foreach (var panel in SkillScrollViewPanels) {
                Destroy(panel);
            }
            SkillScrollViewPanels.Clear();
        }

        private void HandleShopVisibilityToggled(bool currentVisibility, Unit player) {
            if (!currentVisibility) return;
            UpdateSkillScrollView();
        }

        void UpdateSkillScrollView() {
            var player = InGameShopManager.Instance.PurchasingUnit;
            if (player == null) return;

            foreach (var panel in SkillScrollViewPanels) {
                Destroy(panel);
            }
            SkillScrollViewPanels.Clear();
            
            var abilities = player.AbilityComponent.equippedAbilities.Select(x => x.Value).ToList();
            foreach (var ability in abilities) {
                var panel = Instantiate(SkillScrollViewPanelPrefab, gridTransform);
                panel.UpdateSkillScrollViewPanel(ability);
                SkillScrollViewPanels.Add(panel);
            }
        }
    }
}