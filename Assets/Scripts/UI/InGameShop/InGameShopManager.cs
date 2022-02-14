﻿using System;
using Common;
using Units;
using UnityEngine;

namespace UI.InGameShop {
    public class InGameShopManager : MonoBehaviour {
        public InGameShop ShopUI { get; private set; }
        public bool IsPurchasingUnitWithinProximity { get; private set; }
        public Unit PurchasingUnit { get; private set; }
        public Action<bool, Unit> OnShopVisibilityToggled = delegate { };
        public bool isShopVisible => ShopUI != null && ShopUI.gameObject.activeInHierarchy;

        private void Start() {
            try {
                ShopUI = FindObjectOfType<InGameShop>(true);
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
            ShopUI = shopUI.GetComponent<InGameShop>();
            ShopUI.gameObject.SetActive(false);
        }

        public void ToggleVisibility() {
            var previousVisibility = ShopUI.gameObject.activeInHierarchy;
            var currentVisibility = !previousVisibility;
            ShopUI.gameObject.SetActive(currentVisibility);
            Debug.Assert(PurchasingUnit != null);
            OnShopVisibilityToggled(currentVisibility, PurchasingUnit);
        }

        public void PlayerEnteredOrExitedProximity(bool withinProximity, Unit unit) {
            IsPurchasingUnitWithinProximity = withinProximity;
            PurchasingUnit = unit;
        }
    }
}