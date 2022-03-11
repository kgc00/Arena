using System;
using System.Collections.Generic;
using Common;
using Sirenix.Utilities;
using UnityEngine;

namespace Pooling {
    public class ObjectPool : Singleton<MonoBehaviour> {
        private List<PoolModel> models;
        private static Dictionary<string, Pool> pools;

        private void Start() {
            pools = new Dictionary<string, Pool>();
            models ??= Resources.Load<PoolModelSO>($"{Constants.PoolingPath}PoolModelSO").models;
            foreach (var model in models) {
                AddPool(model);
            }
        }

        public static void AddPool(PoolModel poolModel) {
            if (pools.ContainsKey(poolModel.key)) return;
            var pool = new Pool(poolModel);
            pool.pool = new Queue<IPoolable>(pool.maxItemsInPool);
            pools.Add(pool.key, pool);
            for (int i = 0; i < pool.maxItemsInPool; ++i)
                AddOrReturnInstanceToPool(pool.key, CreatePoolable(poolModel.prefab));
        }

        public static void AddOrReturnInstanceToPool(string key, IPoolable poolable) {
            if (!pools.ContainsKey(key)) return;
            var pool = pools[key];
            if (poolable == null || pool.pool.Contains(poolable))
                return;
            if (pool.pool.Count >= pool.maxItemsInPool) {
                Destroy(poolable.Owner);
                return;
            }

            poolable.poolKey = pool.key;
            poolable.inUse = false;
            poolable.HandleReturnToPool();
            poolable.Owner.transform.SetParent(Instance.transform);
            poolable.Owner.gameObject.SetActive(false);
            pool.pool.Enqueue(poolable);
        }

        static IPoolable CreatePoolable(GameObject prefab) {
            var instance = Instantiate(prefab).GetComponent<IPoolable>() ?? throw new Exception("Prefab must implement an IPoolable interface");
            return instance;
        }


        public static IPoolable GetInstanceFromPool(string key) {
            if (!pools.ContainsKey(key))
                return null;
            var pool = pools[key];
            if (pool.pool.Count == 0) {
                AddOrReturnInstanceToPool(pool.key, CreatePoolable(pool.prefab));
                if (pool.pool.Count == 0) return null; // handle creation failure
            }

            var poolable = pool.pool.Dequeue();
            poolable.HandleExitFromPool();
            poolable.inUse = true;
            poolable.Owner.transform.SetParent(null);
            poolable.Owner.SetActive(true);
            return poolable;
        }

        // public static void ReturnInstanceToPool(IPoolable poolable) {
        //     if (pools == null || pools.Count == 0) return;
        //     if (poolable == null || !pools.ContainsKey(poolable.poolKey)) return;
        //     var pool = pools[poolable.poolKey];
        //     add
        //     }
        // }
    }
}