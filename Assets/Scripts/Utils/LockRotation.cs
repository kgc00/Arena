using UnityEngine;

namespace Utils {
    public class LockRotation : MonoBehaviour {
        private Quaternion _originalRotation;

        private void Awake() {
            _originalRotation = transform.rotation;
        }

        private void Update() {
            transform.rotation = _originalRotation;
        }
    }
}