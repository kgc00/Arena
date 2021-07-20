using UnityEngine;

namespace Common {
    public class DestroyOtherOnDestroy : MonoBehaviour {
        private GameObject _linkedGO;
        public void LinkGameObject(GameObject linkedGO) => _linkedGO = linkedGO;
        private void OnDestroy() {
            if (_linkedGO)
                Destroy(_linkedGO);
        }
    }
}