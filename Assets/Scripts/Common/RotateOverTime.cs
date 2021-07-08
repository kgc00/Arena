using UnityEngine;

namespace Common {
    public class RotateOverTime : MonoBehaviour {
        [SerializeField] float _rotationSpeed;
        private void Update() {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * _rotationSpeed);
        }
    }
}