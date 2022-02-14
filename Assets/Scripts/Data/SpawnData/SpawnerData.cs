using System;
using UnityEngine;

namespace Data.SpawnData {
    [CreateAssetMenu(fileName = "Spawner Data", menuName = "ScriptableObjects/Spawns/SpawnerData", order = 0), Serializable]
    public class SpawnerData : ScriptableObject {
        // duration of the vfx / time it takes to spawn in a new unit
        [SerializeField] public float spawnStartupTime;
        public float SpawnStartupTime { 
            get => spawnStartupTime;
            private set => spawnStartupTime = value;
        }
        
        // time to wait after spawning in a unit to spawn in the next one
        [SerializeField] public float delayBetweenUnits;
        public float DelayBetweenUnits { 
            get => delayBetweenUnits;
            private set => delayBetweenUnits = value;
        }
        
        // time to wait between spawning all units in a UnitData
        [SerializeField] public float delayBetweenUnitGroupings;
        public float DelayBetweenUnitGroupings { 
            get => delayBetweenUnitGroupings;
            private set => delayBetweenUnitGroupings = value;
        }
    }
}