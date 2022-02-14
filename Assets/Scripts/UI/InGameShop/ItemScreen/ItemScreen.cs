using Data.Items;
using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.ItemScreen {
    public class ItemScreen : ShopScreen {
        private Unit _purchasingUnit;
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            _purchasingUnit = _inGameShopManager.PurchasingUnit;
        }

        public void HandlePurchase(ItemData model) {
            Debug.Assert(_purchasingUnit != null);
            var (containsEnoughFunds, remainder) = _purchasingUnit.FundsComponent.ContainsEnoughFunds(model.Cost);
            if (!containsEnoughFunds) {
                this.PostNotification(NotificationType.InsufficientFundsForPurchase);
                return;
            }
            _purchasingUnit.FundsComponent.SetBalance(remainder);
            _purchasingUnit.PurchasedItems.Add(model.ItemType);
            this.PostNotification(NotificationType.PurchaseComplete, new PurchaseEvent(model.Cost, model.ItemType.ToString()));
        }
    }
}