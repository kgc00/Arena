using System.Collections.Generic;
using Data.Pickups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawner.Pickups {
    // unused for now - went with drops instead - maybe add in later
    public class PickupSpawner : MonoBehaviour {
        #region Inspector Properties
        [Header("Center"), Range(-20f, 20f), SerializeField]
        private float xPos;

        [Range(-20f, 20f), SerializeField] private float zPos;

        [Header("Size"), Range(-25f, 25f), SerializeField]
        private float xModifier;

        [Range(-25f, 25f), SerializeField] private float zModifier;
        [Range(1f, 50f), SerializeField] private float size = 48f;

        public Vector3 Bounds { get; private set; }
        [Header("Data")] [SerializeField] public int baseDelayBetweenSpawns;
        [SerializeField] public Vector2 randomSpawnDelay;
        #endregion
        
        private float TimeSinceLastSpawn { get; set; }
        private float _spawnInterval;
        private void Start() {
            transform.position = new Vector3(xPos, 0, zPos);
            Bounds = new Vector3(size + xModifier, .1f, size + zModifier);
            RestartSpawnTimer();
        }

        private void Update() {
            TimeSinceLastSpawn += Time.deltaTime;
            if (TimeSinceLastSpawn < _spawnInterval) return;
            HandleAttemptToSpawnPickup();
            RestartSpawnTimer();
        }

        private void HandleAttemptToSpawnPickup() {
            Debug.Log("Spawning Pickup");
            // foreach (var pickupData in spawnableItems) {
            //     var roll = Random.Range(0, 100f);
            //     if (!(roll <= pickupData.dropRate)) continue;
            //     Debug.Log("Spawned pickup: " + pickupData.dropType);
            //     break;
            // }
        }

        private void RestartSpawnTimer() {
            _spawnInterval = baseDelayBetweenSpawns + Random.Range(randomSpawnDelay.x, randomSpawnDelay.y);
            TimeSinceLastSpawn = 0f;
        }
    }
}