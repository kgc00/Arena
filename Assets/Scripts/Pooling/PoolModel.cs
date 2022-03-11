using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling {
    [Serializable]
    public class PoolModel {
        [SerializeField] public int maxItemsInPool;
        [SerializeField] public GameObject prefab;
        [SerializeField] public List<IPoolable> pool;
        [SerializeField] public string key;
    }
}