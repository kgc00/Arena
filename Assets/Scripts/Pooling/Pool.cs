using System.Collections.Generic;
using UnityEngine;

namespace Pooling {
    public class Pool {
        public int maxItemsInPool;
        public readonly GameObject prefab;
        public Queue<IPoolable> pool;
        public string key;

        public Pool(PoolModel poolModel) {
            maxItemsInPool = poolModel.maxItemsInPool;
            prefab = poolModel.prefab;
            pool = new Queue<IPoolable>(poolModel.maxItemsInPool);
            key = poolModel.key;
        }
    }
}