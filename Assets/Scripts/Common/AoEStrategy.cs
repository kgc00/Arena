using System;
using System.Collections;
using UnityEngine;

namespace Common {
    public class AoEStrategy {
        public Func<Collider, Rigidbody, float, Transform, IEnumerator> Strategy;
    }
}