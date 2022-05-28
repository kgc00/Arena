using Data;
using UnityEngine;
using Utils;

namespace UI.Targeting {
    public class ScaleAndPositionArrowUI : MonoBehaviour {
        [SerializeField] private SpriteRenderer image;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 _midpoint;
        private Vector3 _heading;
        private Transform _startTransform;
        private Vector3? _overrideEndLocation;
        TargetingData _targetingData;

        public void SetTargetLocation(TargetingData intentTargetingData, bool isAiSkill) {
            _targetingData = intentTargetingData;
            if (!isAiSkill) return; // quick hack to have the player's arrow always set to mouse pos and the AI set to a static v3
            _overrideEndLocation = intentTargetingData._location;
        }

        private void LateUpdate() {
            if (_startTransform == null) _startTransform = transform.root;
            _startPos = _startTransform.position;
            _startPos.y = 0;
            if (_overrideEndLocation.HasValue) {
                _endPos = _overrideEndLocation.Value;
            } else if (_targetingData != null) {
                _endPos = _targetingData._locationOverrideFromAbility != null
                      ? _targetingData._locationOverrideFromAbility(MouseHelper.GetWorldPosition())
                      : MouseHelper.GetWorldPosition();
            }

            _endPos.y = 0;
            _midpoint = (_startPos + _endPos) / 2;
            _midpoint.y = 0;
            _heading = _endPos - _startPos;
            _heading.y = 0f;
            image.size = new Vector2(5.12f, _heading.magnitude);
            image.transform.position = _midpoint;

            Quaternion rotation = Quaternion.LookRotation(Vector3.up, _heading);
            image.transform.rotation = rotation;
        }

        private void OnDisable() {
            _overrideEndLocation = null;
        }
    }
}