using UnityEngine;

namespace Common {
    [RequireComponent(typeof(ParticleSystem))]
    public class BaseRadiusProvider : MonoBehaviour {
        [HideInInspector] public float baseRadius;
        [SerializeField] public float baseRadiusOverride = -1;

        private void Awake() {
            baseRadius = baseRadiusOverride != -1 ? baseRadiusOverride : GetComponent<ParticleSystem>().main.startSizeMultiplier;
        }
    }
}