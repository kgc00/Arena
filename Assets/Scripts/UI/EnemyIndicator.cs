using System;
using UnityEngine;
using static Utils.MathHelpers;

namespace UI {
    public class EnemyIndicator : MonoBehaviour {
        private bool initialized;
        private bool shouldDestroy;
        private float lifetime;
        public GameObject projectile;
        float timeLeft;
        public void SetScale(float scale) => gameObject.transform.localScale = new Vector3(scale, 0.1f, scale);
        public void SetPosition(Vector3 pos) => gameObject.transform.position = new Vector3(pos.x, -0.5f, pos.z);

        public void SetLifetime(float indLifetime) {
            lifetime = indLifetime;
            timeLeft = lifetime;
        }

        public void SetGameObject(GameObject go) => projectile = go;

        public EnemyIndicator Initialize(Vector3 pos, float scale, float indLifetime) {
            SetPosition(pos);
            SetScale(scale);
            SetLifetime(indLifetime);
            initialized = true;
            return this;
        }

        public EnemyIndicator Initialize(Vector3 pos, float scale, GameObject go) {
            SetPosition(pos);
            SetScale(scale);
            SetGameObject(go);
            initialized = true;
            return this;
        }

        private void Update() {
            if (!initialized) return;

            timeLeft = Clamp(timeLeft - Time.deltaTime, 0, lifetime);
            if (timeLeft > 0 || projectile != null) return;

            shouldDestroy = true;
        }

        private void LateUpdate() {
            if (!initialized || !shouldDestroy) return;

            Destroy(gameObject);
        }
    }
}