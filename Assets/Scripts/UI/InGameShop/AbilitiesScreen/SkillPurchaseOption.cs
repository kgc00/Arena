using Data.Types;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen {
    public class SkillPurchaseOption : MonoBehaviour {
        [SerializeField] private GameObject button;
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            this.AddObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
        }

        private void OnDisable() {
            this.RemoveObserver(HandleSkillScrollViewToggleToggledOn, NotificationType.SkillScrollViewToggleToggledOn);
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
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