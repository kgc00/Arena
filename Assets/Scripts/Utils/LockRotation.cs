using UnityEngine;

namespace Utils {
    public class LockRotation : MonoBehaviour {
        private Quaternion _originalRotation;
        [SerializeField] private bool lookAtCamera;

        private void Awake() {
            _originalRotation = transform.rotation;
        }

        private void Update() {
            if (lookAtCamera && Camera.main != null) {
                transform.LookAt(Camera.main.transform);
            } else {
                transform.rotation = _originalRotation;
            }
        }
    }
}