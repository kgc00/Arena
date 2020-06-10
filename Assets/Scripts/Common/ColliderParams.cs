using UnityEngine;

namespace Common {
    public abstract class ColliderParams { }
    
    public class BoxParams : ColliderParams {
        public Vector3 Bounds { get; private set; }

        public BoxParams(Vector3 bounds) : base() {
            Bounds = bounds;
        }
    }
    
    public class SphereParams : ColliderParams {
        public float Radius { get; private set; }
        public SphereParams(float radius) : base() {
            Radius = radius;
        }
    }
}