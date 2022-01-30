using UnityEngine;

namespace Common {
    [RequireComponent(typeof(ParticleSystem))]
    public class BaseRadiusProvider : MonoBehaviour {
        [HideInInspector] public float baseRadius;

        private void Awake() {
            baseRadius = GetComponent<ParticleSystem>().main.startSizeMultiplier;
        }
    }
}