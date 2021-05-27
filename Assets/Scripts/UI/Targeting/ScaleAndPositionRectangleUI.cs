﻿using UnityEngine;
using Utils;

namespace UI.Targeting {
    public class ScaleAndPositionRectangleUI : MonoBehaviour {
        [SerializeField] private SpriteRenderer image;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 midpoint;
        private Vector3 heading;
        private float _size;
        private Transform startTransform;
        
        public void SetSize(float size) {
            _size = size;
            image.size =  new Vector3(size / 4, size);
        }

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
            image.transform.position = _startPos + heading.normalized * (_size / 2);

            Quaternion rotation = Quaternion.LookRotation(Vector3.up, heading);
            image.transform.rotation = rotation;
        }
    }
}