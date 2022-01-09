using Units;
using UnityEngine;

namespace UI.InGameShop {
    public class ShopArrow : MonoBehaviour {

        [SerializeField] Vector3 originalPosition = new Vector3(0,3.25f,0);
        [SerializeField] float RotationSpeed = 1;
        [SerializeField] private float bobSpeed;
        [SerializeField] private float bobDistance;
        private Unit toFollow;

        private void OnEnable() {
            transform.position = originalPosition;
            transform.localScale = Vector3.one; 
            transform.rotation = Quaternion.identity;
        }

        public void Initialize(Unit unit) {
            toFollow = unit;
        }

        private void Update() {
            if (toFollow == null) {
                gameObject.SetActive(false);
                return;
            }
            transform.position = toFollow.transform.position + originalPosition + new Vector3(0, Mathf.Sin(bobSpeed * Time.time) * bobDistance, 0);
            transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
        }
    }
}