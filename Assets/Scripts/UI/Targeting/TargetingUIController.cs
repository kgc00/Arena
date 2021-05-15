using Data.Types;
using State.PlayerStates;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace UI.Targeting {
    public class TargetingUIController : MonoBehaviour {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private GameObject _circlePrefab;
        [SerializeField] private GameObject _rectanglePrefab;
        private GameObject _arrow;
        private GameObject _circle;
        private GameObject _rectangle;

        private void Awake() {
            this.AddObserver(EnableTargeting, NotificationType.AbilityWillActivate);
            this.AddObserver(DisableTargeting, NotificationType.AbilityDidActivate);
        }

        private void OnDestroy() {
            this.RemoveObserver(EnableTargeting, NotificationType.AbilityWillActivate);
            this.RemoveObserver(DisableTargeting, NotificationType.AbilityDidActivate);
        }

        private void Start() {
            _arrow = Instantiate(_arrowPrefab, transform, true);
            _arrow.SetActive(false);
            _circle = Instantiate(_circlePrefab, transform, true);
            _circle.SetActive(false);
            _rectangle = Instantiate(_rectanglePrefab, transform, true);
            _rectangle.SetActive(false);
        }

        private void EnableTargeting(object notificationName, object eventData) {
            if (!(eventData is PlayerIntent intent)) return;
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Arrow)) _arrow.SetActive(true);
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Circle)) {
                _circle.SetActive(true);
                _circle.SendMessage("SetSize", intent.ability.AreaOfEffectRadius);
            }
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Rectangle)) {
                _rectangle.SetActive(true);
                _rectangle.SendMessage("SetSize", intent.ability.Range);
            }
        }

        private void DisableTargeting(object notificationName, object eventData) {
            if (!(eventData is PlayerIntent intent)) return;
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Arrow)) _arrow.SetActive(false);
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Circle)) _circle.SetActive(false);
            if (intent.ability.IndicatorType.HasFlag(IndicatorType.Rectangle)) _rectangle.SetActive(false);
        }
    }
}