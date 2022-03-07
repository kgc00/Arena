using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common {
    public class ProjectileComponent : MonoBehaviour {
        List<Action<GameObject, GameObject>> onConnected;
        private float speed;
        private float Range;
        private Vector3 InitialPosition;

        private Vector3 Direction { get; set; }
        // private Rigidbody rigidbody;

        //optional param for setting projectile speed
        public void Initialize(Vector3 dir,
            List<Action<GameObject, GameObject>> callbacks,
            float projectileSpeed = default,
            float range = Int16.MaxValue,
            float triggerWidthOverride = -1, 
            bool overrideForwardDir = false) {
            if (overrideForwardDir) {
                gameObject.transform.LookAt(dir);
            }
            var forward = gameObject.transform.forward;
            Direction = new Vector3(forward.x, 0, forward.z);
            onConnected = callbacks;
            speed = projectileSpeed;
            Range = range;
            InitialPosition = gameObject.transform.position;
            if (triggerWidthOverride == -1) return;
            if (!TryGetComponent<BoxCollider>(out var boxCollider)) return;
            var newSize = boxCollider.size;
            newSize.x = triggerWidthOverride;
            boxCollider.size = newSize;
            // rigidbody = GetComponent<Rigidbody>() ?? throw new Exception($"No rigidbody found on {name}");
        }
        void Update() => CheckRange();

        private void CheckRange() {
            if (Vector3.Distance(transform.position, InitialPosition) <= Range) {
                return;
            }

            // to do, maybe some on destroy callback
            Destroy(gameObject);
        }

        void FixedUpdate() => MoveGameObject();
        private void MoveGameObject() => transform.position += Direction * (speed * Time.deltaTime);

        private void OnTriggerEnter(Collider other) {
            // Apply all callbacks in order from lowest to highest
            foreach (var cb in onConnected)
                cb(other.gameObject, gameObject);
        }
    }
}