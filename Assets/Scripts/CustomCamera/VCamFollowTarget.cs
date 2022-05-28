using UnityEngine;

namespace CustomCamera {
    public class VCamFollowTarget : MonoBehaviour {
        [SerializeField] public float cameraMaxFollowDistance;
        [SerializeField] private bool drawGizmos;
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (!drawGizmos) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
#endif
}