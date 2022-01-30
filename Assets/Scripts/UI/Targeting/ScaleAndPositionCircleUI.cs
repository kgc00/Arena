using Data;
using UnityEngine;
using Utils;

namespace UI.Targeting {
    public class ScaleAndPositionCircleUI : MonoBehaviour {
        [SerializeField] private SpriteRenderer image;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 _midpoint;
        private Vector3 _heading;
        private float _size;
        private Transform _startTransform;
        private TargetingData _targetingData;

        public void SetSizeAndLocation(float size, TargetingData targetingData) {
            _size = size;
            image.size = Vector3.one * (size * 2);
            _targetingData = targetingData;
        }

        private void Update() {
            if (_startTransform == null) _startTransform = transform.root;
            _startPos = _startTransform.position;
            _startPos.y = 0;
            _endPos = _targetingData._behavior == TargetingBehavior.CursorLocation
                ? MouseHelper.GetWorldPosition()
                : _targetingData._location;
            _endPos.y = 0;
            _midpoint = (_startPos + _endPos) / 2;
            _midpoint.y = 0;
            _heading = _endPos - _startPos;
            _heading.y = 0f;
            image.transform.position = _endPos;

            Quaternion rotation = Quaternion.LookRotation(Vector3.up, _heading);
            image.transform.rotation = rotation;
        }
    }
}