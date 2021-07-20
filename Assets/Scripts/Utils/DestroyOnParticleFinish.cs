using System.Collections;
using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyOnParticleFinish : MonoBehaviour {
        
        ParticleSystem _particleSystem;
        public void Start(){
            _particleSystem = GetComponent<ParticleSystem>();
            StartCoroutine(DestroySelf());
        }

        private IEnumerator DestroySelf() {
            yield return new WaitForSeconds(_particleSystem.main.duration - Time.deltaTime);
            Destroy(gameObject);
        }

        // private void OnGUI() {
        //     GUILayout.BeginArea(new Rect(new Vector2(250,15),new Vector2(150,300)));
        //     GUILayout.Box(_particleSystem.time.ToString());
        //     GUILayout.Box(_particleSystem.isPlaying.ToString());
        //     GUILayout.Box(_particleSystem.IsAlive().ToString());
        //     GUILayout.Box(_particleSystem.isEmitting.ToString());
        //     GUILayout.EndArea();
        // }
    }
}