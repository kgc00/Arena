using UnityEngine;

namespace Common {
    public class SetParticleData : MonoBehaviour {
        public void Initialize(float duration, float areaOfEffectRadius) { 
            var topLevelPS = GetComponent<ParticleSystem>();
            var baseRadius = GetComponentInChildren<BaseRadiusProvider>().baseRadius;
            foreach (var ps in GetComponentsInChildren<ParticleSystem>()) {
                if (ps == topLevelPS) continue;
                ps.Stop();
                var main = ps.main;
                main.duration = duration;
                main.startSizeMultiplier = areaOfEffectRadius / baseRadius * main.startSizeMultiplier;
                ps.Play();
            }
        }
    }
}