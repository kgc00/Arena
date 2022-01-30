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

        private void Update() {
            if (_startTransform == null) _startTransform = transform.root;
            _startPos = _startTransform.position;
            _startPos.y = 0;
            _endPos = MouseHelper.GetWorldPosition();
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
    }
}