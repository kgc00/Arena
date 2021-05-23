using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyGameobjectOnParticleFinish : MonoBehaviour {
        private ParticleSystem _particleSystem;

        private void Start() {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update() {
            if(_particleSystem.isPlaying)  return;
            
            Destroy(gameObject);
        }
    }
}