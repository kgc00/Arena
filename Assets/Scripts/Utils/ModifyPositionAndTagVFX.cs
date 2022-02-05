using System.Collections;
using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(ParticleSystem))]
    public class ModifyPositionAndTagVFX : MonoBehaviour {
        private Vector3 _originalPosition;
        private ParticleSystem _particleSystem;

        private void Awake() {
            _particleSystem = GetComponent<ParticleSystem>();
            _originalPosition = transform.position;
            
        }

        private void Start() {
            StartCoroutine(PlayRepeatingWithOffset());
        }

        IEnumerator PlayRepeatingWithOffset() {
            var duration = _particleSystem.main.duration;
            while (_particleSystem != null) {
                yield return new WaitForSeconds(duration);
                var range = 1.5f;
                Vector3 randomOffset = new Vector3(Random.Range(-range,range), Random.Range(-range,range), Random.Range(-range,range));
                transform.position = gameObject.transform.root.position + randomOffset;
            }
        }
    }
}