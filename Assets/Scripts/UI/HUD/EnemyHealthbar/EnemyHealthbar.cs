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
        private EnemyHealthbarHolder _enemyHealthbarHolder;

        private void Start() {
            _enemyHealthbarHolder = EnemyHealthbarHolder.Instance; 
            _owner = transform.root.GetComponent<Unit>();
            _healthFill = healthFillGo.GetComponent<Image>();
            UpdateHealthValue();

            HealthComponent.OnHealthChanged += UpdateHealthValue;
            Unit.OnDeath += Cleanup;
            transform.SetParent(_enemyHealthbarHolder.transform);
        }

        private void Cleanup(Unit unit) {
            if (unit != _owner) return;
            Destroy(healthbar);
            Destroy(this);
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
            if (_owner != null) {
                transform.position = _owner.transform.position - Vector3.forward;
            }
        }
    }
}