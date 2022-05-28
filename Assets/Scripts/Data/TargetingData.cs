using System;
using UnityEngine;

namespace Data {
    [Serializable]
    public class TargetingData {
        [SerializeField] public TargetingBehavior _behavior;
        [SerializeField] public Vector3 _location;
        public Func<Vector3, Vector3> _locationOverrideFromAbility;
        public TargetingData(TargetingBehavior behavior, Vector3 location, Func<Vector3, Vector3> locationOverrideFromAbility = null) {
            _behavior = behavior;
            _location = location;
            _locationOverrideFromAbility = locationOverrideFromAbility;
        }
    }
}