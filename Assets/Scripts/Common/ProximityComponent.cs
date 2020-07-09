using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common {
    public class ProximityComponent : MonoBehaviour {
        private Vector3 destination;
        private float threshold = 1;
        public bool IsLive { get; private set; } = true;
        private List<Action<Vector3>> onDestinationReached;

        public ProximityComponent Initialize(Vector3 destinationPos, List<Action<Vector3>> callbacks) {
            destination = new Vector3(destinationPos.x, 0,destinationPos.z);
            onDestinationReached = callbacks;
            IsLive = true;
            return this;
        }

        private void Update() {
            if (!IsLive) return;
            
            var pos2d = gameObject.transform.position;
            pos2d.y = 0;
            
            var dist = Vector3.Distance(destination, pos2d);
            if (dist > threshold) return;

            HandleDestinationReached();
        }

        private void HandleDestinationReached() {
            foreach (var cb in onDestinationReached)
                cb(transform.position);

            IsLive = false;
        }

        private void LateUpdate() {
            if (IsLive) return;
            
            Destroy(gameObject);
        }

        public void SetInactive() => IsLive = false;
    }
}