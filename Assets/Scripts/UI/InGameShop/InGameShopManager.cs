using Common;
using UnityEngine;

namespace UI.InGameShop {
    public class InGameShopManager : Singleton<InGameShopManager> {
        public GameObject _shopUI { get; private set; }
        public bool _playerWithinProximity { get; private set; }

        private void Start() {
            try {
                _shopUI = FindObjectOfType<InGameShop>(true).gameObject;
            }
            catch {
                // ignored
            }

            if (_shopUI != null) return;
            var canvas = FindObjectOfType<Canvas>();
            if (_shopUI != null || canvas == null) return;
            var shopUI = Instantiate(Resources.Load<RectTransform>($"{Constants.UIPath}In Game Shop"),
                Vector3.one,
                Quaternion.identity, canvas.transform);
            shopUI.anchoredPosition = Vector2.zero;
            _shopUI = shopUI.gameObject;
            _shopUI.SetActive(false);
        }

        public void ToggleVisibility() {
            if (_playerWithinProximity) _shopUI.SetActive(!_shopUI.activeInHierarchy);
        }

        public void PlayerEnteredProximity(bool withinProximity) {
            _playerWithinProximity = withinProximity;
            if (!_playerWithinProximity) _shopUI.SetActive(false);
        }
    }
}