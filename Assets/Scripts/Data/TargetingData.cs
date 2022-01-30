using System;
using UnityEngine;

namespace Data {
    [Serializable]
    public class TargetingData {
        [SerializeField] public TargetingBehavior _behavior;
        [SerializeField] public Vector3 _location;
        public TargetingData(TargetingBehavior behavior, Vector3 location) {
            _behavior = behavior;
            _location = location;
        }
    }
}