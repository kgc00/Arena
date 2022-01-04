using Data.Types;
using UI.InGameShop;
using Units;
using UnityEngine;

namespace Arena {
    public class ShopTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.transform.root.TryGetComponent(out Unit unit) && unit.Owner.ControlType == ControlType.Local) {
                InGameShopManager.Instance.PlayerEnteredOrExitedProximity(true, unit);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.transform.root.TryGetComponent(out Unit unit) && unit.Owner.ControlType == ControlType.Local) {
                InGameShopManager.Instance.PlayerEnteredOrExitedProximity(false, unit);
            }
        }
    }
}