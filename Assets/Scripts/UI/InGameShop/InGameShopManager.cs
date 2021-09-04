using Common;
using UnityEngine;

namespace UI.InGameShop {
    public class InGameShopManager : Singleton<InGameShopManager> {
        private GameObject _shopUI;

        private void Start() {
            try {
                _shopUI = FindObjectOfType<InGameShop>(true).gameObject;
            }
            catch {
                // ignored
            }

            if (_shopUI == null) {
                var canvas = FindObjectOfType<Canvas>();
                if (_shopUI == null) {
                    _shopUI = Instantiate(Resources.Load<GameObject>($"{Constants.UIPath}/InGameShop"));
                    if (canvas != null) {
                        _shopUI.transform.SetParent(canvas.transform);
                    }
                }
            }

            Debug.Assert(_shopUI != null);
            _shopUI.SetActive(false);
        }

        public void ToggleVisibility() {
            _shopUI.SetActive(!_shopUI.activeInHierarchy);
        }
    }
}