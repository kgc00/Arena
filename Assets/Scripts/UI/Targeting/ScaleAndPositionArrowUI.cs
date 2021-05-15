using UnityEngine;
using Utils;

namespace UI.Targeting {
    public class ScaleAndPositionArrowUI : MonoBehaviour {
        [SerializeField] private SpriteRenderer image;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 midpoint;
        private Vector3 heading;
        private Transform startTransform;

        private void Update() {
            if (startTransform == null) startTransform = Locator.GetClosestPlayerUnit(Vector3.zero) ?? transform;
            _startPos = startTransform.position;
            _startPos.y = 0;
            _endPos = MouseHelper.GetWorldPosition();
            _endPos.y = 0;
            midpoint = (_startPos + _endPos) / 2;
            midpoint.y = 0;
            heading = _endPos - _startPos;
            heading.y = 0f;
            image.size = new Vector2(5.12f, heading.magnitude);
            image.transform.position = midpoint;

            Quaternion rotation = Quaternion.LookRotation(Vector3.up, heading);
            image.transform.rotation = rotation;
        }
        
        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 100, 100, 100));
            GUILayout.Box(heading.magnitude.ToString() );
            GUILayout.EndArea();
        }
    }
}