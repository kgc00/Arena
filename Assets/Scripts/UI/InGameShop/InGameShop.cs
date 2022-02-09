using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace UI.InGameShop {
    public class InGameShop : MonoBehaviour {
        [SerializeField] private ShopArrow ArrowPrefab;
        [HideInInspector] public ShopArrow ArrowObject;
        private WindowType _activeWindow;
        [SerializeField] private ShopScreen AbilityWindow;
        [SerializeField] private ShopScreen StatsWindow;
        [SerializeField] private ShopScreen ItemsWindow;
        private Dictionary<WindowType, ShopScreen> _windows;

        private void OnEnable() {
            _windows ??= new Dictionary<WindowType, ShopScreen> {
                {WindowType.Abilities, AbilityWindow},
                {WindowType.Stats, StatsWindow},
                {WindowType.Items, ItemsWindow},
            };
            Initialize();
            InGameShopManager.Instance.OnShopVisibilityToggled += HandleVisibilityToggled;
        }

        private void OnDisable() {
            InGameShopManager.Instance.OnShopVisibilityToggled -= HandleVisibilityToggled;
        }

        public void ToggleActiveWindow(string typeAsString) {
            if (!Enum.TryParse(typeAsString, out WindowType windowType)) return;
            foreach (var window in _windows) {
                window.Value.gameObject.SetActive(window.Key == windowType);
            }
            _activeWindow = windowType;
        }

        private void HandleVisibilityToggled(bool currentVisibility, Unit purchasingUnit) {
            if (!currentVisibility) return;
            _windows[_activeWindow].gameObject.SetActive(true);
        }

        public void Initialize() {
            if (ArrowObject == null) {
                ArrowObject = Instantiate(ArrowPrefab);
                ArrowObject.gameObject.SetActive(false);
            }
        }
    }
}