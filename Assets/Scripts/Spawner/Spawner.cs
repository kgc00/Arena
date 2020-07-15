using System;
using System.Collections;
using System.Linq;
using Data;
using Data.SpawnData;
using Data.Types;
using Players;
using Units;
using UnityEngine;
using Utils;

namespace Spawner {
    public class Spawner : MonoBehaviour {
        #region Properties

        [Header("Center"), Range(-20f, 20f), SerializeField] 
        private float xPos;

        [Range(-20f, 20f), SerializeField] private float zPos;

        [Header("Size"), Range(-25f, 25f), SerializeField] 
        private float xModifier;

        [Range(-25f, 25f), SerializeField] private float zModifier;
        [Range(1f, 50f), SerializeField] private float size = 48f;

        [Header("Data"), SerializeField]  public Intervals interval;
        [SerializeField] public HordeSpawnData hordeSpawnData;
        public Vector3 Bounds { get; private set; }
        [SerializeField] public Interval Interval { get; private set; }
        [SerializeField] public WaveHandler Handler { get; private set; }
        public Player OwningPlayer { get; private set; }

        [SerializeField] private SpawnerData model;

        #endregion

        private void OnEnable() {
            if (hordeSpawnData == null) hordeSpawnData = PersistentData.Instance.HordeModel;

            transform.position = new Vector3(xPos, 0, zPos);
            Bounds = new Vector3(size + xModifier, 1f, size + zModifier);
            // WILL BREAK IF WE ADD MORE THAN ONE AI PLAYER
            OwningPlayer = FindObjectsOfType<Player>().FirstOrDefault(player => player.ControlType == ControlType.Ai);
            if (Handler == null) Handler = new WaveHandler(hordeSpawnData, this);
            if (Interval == null) {
                IEnumerator WrappedSpawn() {
                    return Handler.Spawn(model.spawnStartupTime, model.delayBetweenSpawns);
                }

                Interval = SpawnHelper.IntervalFromType(interval, gameObject)
                    .Initialize(WrappedSpawn, this, model.delayBetweenWaves, model.delayBetweenSpawns,
                        model.spawnStartupTime);
            }

            // StartCoroutine(Handler.Spawn(2f, 0));
        }

        public void StartSpawnCoroutine(float delay, GameObject spawnVfx, Func<Unit> spawnUnit) {
            StartCoroutine(EnemySpawnCoroutine(delay, spawnVfx, spawnUnit));
        }

        private IEnumerator EnemySpawnCoroutine(float delay, GameObject spawnVfx, Func<Unit> spawnUnit) {
            yield return new WaitForSeconds(delay);
            Destroy(spawnVfx);

            spawnUnit();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            transform.position = new Vector3(xPos, 0, zPos);
            var debugPos = new Vector3(size + xModifier, 1f, size + zModifier);
            Gizmos.DrawWireCube(
                transform.position,
                debugPos
            );
        }
#endif
    }
}