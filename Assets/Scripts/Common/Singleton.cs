using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }

                return _instance;
            }
        }


        private void Awake ()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            } else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}