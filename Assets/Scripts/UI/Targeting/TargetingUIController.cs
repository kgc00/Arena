using Common;
using Data.Types;
using State;
using State.PlayerStates;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace UI.Targeting {
    public class TargetingUIController : MonoBehaviour {
        private GameObject _arrowPrefab;
        private GameObject _circlePrefab;
        private GameObject _rectanglePrefab;
        private GameObject _arrow;
        private ScaleAndPositionCircleUI _circle;
        private ScaleAndPositionRectangleUI _rectangle;
        private bool _initialized;
        public Unit Owner { get; private set; }

        private void Awake() {
            this.AddObserver(EnableTargeting, NotificationType.AbilityWillActivate);
            this.AddObserver(DisableTargeting, NotificationType.AbilityDidActivate);
        }

        private void OnDestroy() {
            this.RemoveObserver(EnableTargeting, NotificationType.AbilityWillActivate);
            this.RemoveObserver(DisableTargeting, NotificationType.AbilityDidActivate);
        }

        public TargetingUIController Initialize(Unit unit) {
            Owner = unit;
            if (Owner.Owner.ControlType == ControlType.Ai) {
                _arrowPrefab = Resources.Load<GameObject>($"{Constants.PrefabsPath}targeting_arrow_enemy");
                _circlePrefab = Resources.Load<GameObject>($"{Constants.PrefabsPath}targeting_circle_enemy");
                _rectanglePrefab = Resources.Load<GameObject>($"{Constants.PrefabsPath}targeting_square_enemy");
                
            } else {
                _arrowPrefab = Resources.Load<GameObject>($"{Constants.PrefabsPath}targeting_arrow");
                _circlePrefab = Resources.Load<GameObject>($"{Constants.PrefabsPath}targeting_circle");
                _rectanglePrefab = Resources.Load<GameObject>($"{Constants.PrefabsPath}targeting_square");
            }
            _arrow = Instantiate(_arrowPrefab, transform, true);
            _arrow.SetActive(false);
            _circle = Instantiate(_circlePrefab, transform, true).GetComponent<ScaleAndPositionCircleUI>();
            _circle.gameObject.SetActive(false);
            _rectangle = Instantiate(_rectanglePrefab, transform, true).GetComponent<ScaleAndPositionRectangleUI>();
            _rectangle.gameObject.SetActive(false);
            _initialized = true;
            return this;
        }

        private void EnableTargeting(object notificationName, object eventData) {
            if (!_initialized) return;
            if (!(eventData is UnitIntent intent) || !Equals(intent.unit, Owner)) return;
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Arrow)) _arrow.SetActive(true);
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Circle)) {
                _circle.gameObject.SetActive(true);
                _circle.SetSizeAndLocation(intent.ability.AreaOfEffectRadius, intent.targetingData);
            }
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Rectangle)) {
                _rectangle.gameObject.SetActive(true);
                _rectangle.SetSizeAndLocation(intent.ability.Range, intent.targetingData);
            }
        }

        private void DisableTargeting(object notificationName, object eventData) {
            if (!_initialized) return;
            if (!(eventData is UnitIntent intent) || !Equals(intent.unit, Owner)) return;
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Arrow)) _arrow.SetActive(false);
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Circle)) _circle.gameObject.SetActive(false);
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Rectangle)) _rectangle.gameObject.SetActive(false);
        }
    }
}