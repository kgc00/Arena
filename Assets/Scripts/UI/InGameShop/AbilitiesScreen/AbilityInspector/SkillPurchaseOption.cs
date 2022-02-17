using Data.Types;
using UI.InGameShop.AbilitiesScreen.SkillScrollView;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen.AbilityInspector {
    public class SkillPurchaseOption : MonoBehaviour {
        [SerializeField] private GameObject button;
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            this.AddObserver(HandleLockedSkillScrollViewInspected, NotificationType.LockedSkillInspected);
            this.AddObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void OnDisable() {
            this.AddObserver(HandleLockedSkillScrollViewInspected, NotificationType.LockedSkillInspected);
            this.RemoveObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void HandleLockedSkillScrollViewInspected(object sender, object args) {
            if (args is LockedSkillInspectedEvent lockedSkillInspectedEvent) {
                button.SetActive(!lockedSkillInspectedEvent.Model.unlocked);
            }
        }

        private void HandlePurchase(object sender, object args) {
            button.SetActive(false);
        }

        private void HandleSkillScrollViewToggleToggledOn(object sender, object args) {
            if (!(args is SkillScrollViewToggleEvent toggleEvent)) return;
            var player = _inGameShopManager.PurchasingUnit;
            if (player == null) return;
            button.SetActive(!toggleEvent.IsPurchased);
        }
    }
}