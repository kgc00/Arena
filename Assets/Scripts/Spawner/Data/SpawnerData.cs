using System;
using UnityEngine;

namespace Spawner.Data {
    [CreateAssetMenu(fileName = "Spawner Data", menuName = "ScriptableObjects/Spawns/SpawnerData", order = 0), Serializable]
    public class SpawnerData : ScriptableObject {
        [SerializeField] public float spawnStartupTime;
        public float SpawnStartupTime { 
            get => spawnStartupTime;
            private set => spawnStartupTime = value;
        }
        [SerializeField] public float delayBetweenSpawns;
        public float DelayBetweenSpawns { 
            get => delayBetweenSpawns;
            private set => delayBetweenSpawns = value;
        }
        [SerializeField] public float delayBetweenWaves;
        public float DelayBetweenWaves { 
            get => delayBetweenWaves;
            private set => delayBetweenWaves = value;
        }
    }
}