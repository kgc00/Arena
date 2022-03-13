using Components;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.EnemyHealthbar {
    public class EnemyHealthbar : MonoBehaviour {
        private Unit _owner;
        [SerializeField] private GameObject healthFillGo;
        private Image _healthFill;
        [SerializeField] private GameObject healthbar;
        [SerializeField] private Vector3 rotationOffset;
        [SerializeField] Vector3 offset = Vector3.forward;
        private Quaternion _originalRotation;
        [SerializeField] private float yOffset = .1f;
        
        public void Initialize() {
            _originalRotation = Quaternion.Euler(rotationOffset);
            _owner = transform.root.GetComponent<Unit>();
            _healthFill = healthFillGo.GetComponent<Image>();
            UpdateHealthValue();
            HealthComponent.OnHealthChanged += UpdateHealthValue;
        }

        public void Unsubscribe() {
            HealthComponent.OnHealthChanged -= UpdateHealthValue;
        }

        private void UpdateHealthValue(Unit u, float arg2) {
            if (u == _owner)
                UpdateHealthValue();
        }

        void UpdateHealthValue() {
            if (_owner == null || _owner.HealthComponent == null) return;
            var fillAmount = _owner.HealthComponent.CurrentHp / _owner.HealthComponent.MaxHp;
            healthbar.SetActive(fillAmount < 1);
            _healthFill.fillAmount = fillAmount;
        }

        private void Update() {
            transform.rotation = _originalRotation;
            if (_owner == null) return;
            var ownerPos = _owner.transform.position;
            transform.position = new Vector3(ownerPos.x,yOffset,ownerPos.z) - offset;
        }
    }
}