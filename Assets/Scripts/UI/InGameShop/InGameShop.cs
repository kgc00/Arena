using System;
using System.Collections.Generic;
using Common;
using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop {
    public class InGameShop : MonoBehaviour {
        private WindowType _activeWindow;
        [SerializeField] private ShopScreen AbilityWindow;
        [SerializeField] private ShopScreen StatsWindow;
        [SerializeField] private ShopScreen ItemsWindow;
        private Dictionary<WindowType, ShopScreen> _windows;
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            _windows ??= new Dictionary<WindowType, ShopScreen> {
                {WindowType.Abilities, AbilityWindow},
                {WindowType.Stats, StatsWindow},
                {WindowType.Items, ItemsWindow},
            };
            _inGameShopManager.OnShopVisibilityToggled += HandleVisibilityToggled;
        }
        
        private void Start() {
            _activeWindow = WindowType.Abilities;
            foreach (var window in _windows) {
                window.Value.gameObject.SetActive(window.Key == _activeWindow);
            }
        }

        private void OnDisable() {
            _inGameShopManager.OnShopVisibilityToggled -= HandleVisibilityToggled;
        }

        public void ToggleActiveWindow(string typeAsString) {
            if (!Enum.TryParse(typeAsString, out WindowType windowType)) return;
            foreach (var window in _windows) {
                window.Value.gameObject.SetActive(window.Key == windowType);
            }

            _activeWindow = windowType;
            this.PostNotification(NotificationType.DidToggleShopTab);
        }

        private void HandleVisibilityToggled(bool currentVisibility, Unit purchasingUnit) {
            if (!currentVisibility) return;
            _windows[_activeWindow].gameObject.SetActive(true);
        }

        public void CloseShop() {
            this.PostNotification(NotificationType.DidClickShopButton);
            _inGameShopManager.ToggleVisibility();
        }
    }
}