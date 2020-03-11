using System;
using UnityEngine;

namespace Utils
{

    [Serializable]
    public class MyDictionary1 : SerializableDictionary<string, int> { }

    [Serializable] public class MyDictionary2 : SerializableDictionary<KeyCode, GameObject> { }
 
    // [CreateAssetMenu(fileName = "TEST", menuName = "ScriptableObjects/TEST", order = 1), SerializeField]
    public class Test : MonoBehaviour
    {
        public MyDictionary1 dictionary1;
        public MyDictionary2 dictionary2;
    }
}