using Data.Types;
using UI.InGameShop;
using Units;
using UnityEngine;

namespace Arena {
    public class ShopTrigger : MonoBehaviour {
        private InGameShopManager _inGameShopManager;

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.root.TryGetComponent(out Unit unit) && unit.Owner.ControlType == ControlType.Local) {
                _inGameShopManager.PlayerEnteredOrExitedProximity(true, unit);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.transform.root.TryGetComponent(out Unit unit) && unit.Owner.ControlType == ControlType.Local) {
                _inGameShopManager.PlayerEnteredOrExitedProximity(false, unit);
            }
        }
    }
}