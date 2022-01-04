using System;
using Common;
using Units;
using UnityEngine;

namespace UI.InGameShop {
    public class InGameShopManager : Singleton<InGameShopManager> {
        public GameObject ShopUI { get; private set; }
        public bool IsPlayerWithinProximity { get; private set; }
        public Unit Player { get; private set; }
        public Action<bool, Unit> OnShopVisibilityToggled = delegate { };

        private void Start() {
            try {
                ShopUI = FindObjectOfType<InGameShop>(true).gameObject;
            }
            catch {
                // ignored
            }

            if (ShopUI != null) return;
            var canvas = FindObjectOfType<Canvas>();
            if (ShopUI != null || canvas == null) return;
            var shopUI = Instantiate(Resources.Load<RectTransform>($"{Constants.UIPath}In Game Shop"),
                Vector3.one,
                Quaternion.identity, canvas.transform);
            shopUI.anchoredPosition = Vector2.zero;
            ShopUI = shopUI.gameObject;
            ShopUI.SetActive(false);
        }

        public void ToggleVisibility() {
            var previousVisibility = ShopUI.activeInHierarchy;
            var currentVisibility = !previousVisibility;
            ShopUI.SetActive(currentVisibility);
            Debug.Assert(Player != null);
            OnShopVisibilityToggled(currentVisibility, Player);
        }

        public void PlayerEnteredOrExitedProximity(bool withinProximity, Unit unit) {
            IsPlayerWithinProximity = withinProximity;
            Player = unit;
            if (!IsPlayerWithinProximity && ShopUI.activeInHierarchy) ToggleVisibility();
        }
    }
}