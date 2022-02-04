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

        private void Start() {
            _originalRotation = Quaternion.Euler(rotationOffset);
            _owner = transform.root.GetComponent<Unit>();
            _healthFill = healthFillGo.GetComponent<Image>();
            UpdateHealthValue();

            HealthComponent.OnHealthChanged += UpdateHealthValue;
            Unit.OnDeath += Cleanup;
        }

        private void Cleanup(Unit unit) {
            if (unit != _owner) return;
            Destroy(gameObject);
        }

        private void UpdateHealthValue(Unit u, float arg2) {
            if (u == _owner)
                UpdateHealthValue();
        }

        void UpdateHealthValue() {
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