using UI.InGameShop;
using UnityEngine;

namespace Arena {
    public class ShopTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            InGameShopManager.Instance.PlayerEnteredProximity(true);
        }

        private void OnTriggerExit(Collider other) {
            InGameShopManager.Instance.PlayerEnteredProximity(false);
        }
    }
}