using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance { get; private set; }

        protected virtual void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}