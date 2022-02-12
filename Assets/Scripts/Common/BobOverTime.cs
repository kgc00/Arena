using UnityEngine;

namespace Common {
    public class BobOverTime : MonoBehaviour {
        [SerializeField] private float bobSpeed;
        [SerializeField] private float bobDistance;
        private Vector3 _originalPosition;

        private void OnEnable() {
            _originalPosition = transform.position;
        }

        private void Update() {
            var bobValue = Mathf.Sin(bobSpeed * Time.time) * bobDistance + _originalPosition.y;
            var newPos = transform.position;
            newPos.y = bobValue;
            transform.position = newPos;
        }
    }
}