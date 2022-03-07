using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Data.Types;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public class SkillScrollView : MonoBehaviour {
        [SerializeField] private Transform gridTransform;
        [SerializeField] private UnlockedSkillScrollViewPanel UnlockedSkillScrollViewPanelPrefab;
        [SerializeField] private ToggleGroup _toggleGroup;
        public static ToggleGroup ToggleGroup;
        [SerializeField] private List<SkillScrollViewPanel> SkillScrollViewPanels;
        [SerializeField] private ScrollRect _scrollRect;
        private InGameShopManager _inGameShopManager;
        [SerializeField] private LockedSkillScrollViewPanel LockedSkillScrollViewPanelPrefab;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }

            SkillScrollViewPanels = new List<SkillScrollViewPanel>();
            ToggleGroup = _toggleGroup;
            UpdateSkillScrollView();
            
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
            
            StartCoroutine(DelayInspectToggle());
        }

        private IEnumerator DelayInspectToggle() {
            yield return new WaitForEndOfFrame();
            SkillScrollViewPanels[0].InspectAbility(true);
        }

        private void OnDisable() {
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
            
            foreach (var panel in SkillScrollViewPanels) {
                Destroy(panel.gameObject);
            }

            SkillScrollViewPanels.Clear();
        }


        private void HandlePurchase(object sender, object args) {
            if (!(args is PurchaseEvent purchaseEvent)) return;
            if (purchaseEvent.Type != PurchaseEvent.PurchaseType.SkillUnlock) return;
            if (!Enum.TryParse(purchaseEvent.Name, out AbilityType type)) return;
            int index = 0;
            SkillScrollViewPanel oldPanel = null;
            UnlockedSkillScrollViewPanel panel = null;
            for (var i = 0; i < SkillScrollViewPanels.Count; i++) {
                var skillScrollViewPanel = SkillScrollViewPanels[i];
                if (skillScrollViewPanel.AssociatedAbility.Type != type) continue;
                index = i;
                panel = Instantiate(UnlockedSkillScrollViewPanelPrefab, gridTransform);
                panel.UpdateSkillScrollViewPanel(skillScrollViewPanel.AssociatedAbility);
                oldPanel = skillScrollViewPanel;
                break;
            }

            if (panel == null) return;
            SkillScrollViewPanels.Remove(oldPanel);
            Destroy(oldPanel.gameObject);
            SkillScrollViewPanels.Insert(index, panel);
            panel.transform.SetSiblingIndex(index);
            panel.InspectAbility();
        }

        void UpdateSkillScrollView() {
            var player = _inGameShopManager.PurchasingUnit;
            if (player == null) return;

            foreach (var panel in SkillScrollViewPanels) {
                Destroy(panel.gameObject);
            }

            SkillScrollViewPanels.Clear();

            var abilities = player.AbilityComponent.equippedAbilitiesByButton.Select(x => x.Value).ToList();
            foreach (var ability in abilities) {
                if (ability.Unlocked) {
                    var panel = Instantiate(UnlockedSkillScrollViewPanelPrefab, gridTransform);
                    panel.UpdateSkillScrollViewPanel(ability);
                    SkillScrollViewPanels.Add(panel);
                }
                else {
                    var panel = Instantiate(LockedSkillScrollViewPanelPrefab, gridTransform);
                    panel.UpdateSkillScrollViewPanel(ability);
                    SkillScrollViewPanels.Add(panel);
                }
            }
        }
    }
}