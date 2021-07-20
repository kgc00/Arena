using UnityEngine;

namespace Common {
    public class SetParticleDuration : MonoBehaviour {
        public float Duration { get; set; } = -1;
        
        private void Start() {
            Debug.Assert(Duration != -1);
            var topLevelPS = GetComponent<ParticleSystem>();
            foreach (var ps in GetComponentsInChildren<ParticleSystem>()) {
                if ((ps) == topLevelPS) continue;
                ps.Stop();
                var main = ps.main;
                main.duration = Duration;
                ps.Play();
            }
        }
    }
}