using UnityEngine;

namespace Common {
    public class ScaleOverTime : MonoBehaviour {
        [SerializeField] private float _scaleAmount;

        private void Update() {
            transform.localScale *= _scaleAmount * Time.deltaTime;
        }
    }
}