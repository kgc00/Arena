using UnityEngine;

namespace Extensions {
    public static class Vector3Extensions {
        public static Vector3 WithYValue(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
        public static Vector3 WithoutY(this Vector3 v) => WithYValue(v, 0);
    }
}