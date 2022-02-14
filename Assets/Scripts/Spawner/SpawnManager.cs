using System;
using System.Collections;
using System.Linq;
using Arena;
using Data;
using Data.SpawnData;
using Data.Types;
using Players;
using Units;
using UnityEngine;
using Utils;

namespace Spawner {
    public class SpawnManager : MonoBehaviour {
        #region Properties

        [Header("Center"), Range(-20f, 20f), SerializeField]
        private float xPos;

        [Range(-20f, 20f), SerializeField] private float zPos;

        [Header("Size"), Range(-25f, 25f), SerializeField]
        private float xModifier;

        [Range(-25f, 25f), SerializeField] private float zModifier;
        [Range(1f, 50f), SerializeField] private float size = 48f;

        [SerializeField] public WaveSpawnData waveSpawnData;
        public Vector3 Bounds { get; private set; }
        private WaveSpawner WaveSpawner { get; set; }
        [SerializeField] public Player owningPlayer;

        [SerializeField] private SpawnerData model;

        #endregion
        private Coroutine _spawnCrt;
        private ArenaManager _arenaManager;

        private void OnEnable() {
            Unit.OnDeath += HandleUnitDeath;
        }

        private void OnDisable() {
            Unit.OnDeath -= HandleUnitDeath;
        }

        private void HandleUnitDeath(Unit unit) {
            if (unit.Owner.ControlType != ControlType.Ai) return;
            StartCoroutine(CheckWaveClearedCRT(unit.Owner));
        }

        private IEnumerator CheckWaveClearedCRT(Player player) {
            yield return new WaitForSeconds(.2f);
            if (player.Units.Count > 0 || _spawnCrt != null) yield break;
            HandleWavesCleared();
        }

        private void Start() {
            transform.position = new Vector3(xPos, 0, zPos);
            Bounds = new Vector3(size + xModifier, .1f, size + zModifier);

            WaveSpawner ??= new WaveSpawner(this);
            StartSpawn();
        }

        public void StartSpawn() {
            Debug.Assert(_spawnCrt == null);
            waveSpawnData = FindObjectOfType<ArenaData>().CurrentWaveModel[owningPlayer.ControlType];
            _spawnCrt = StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave() {
            yield return StartCoroutine
            (WaveSpawner.Spawn(waveSpawnData, model.spawnStartupTime,
                model.delayBetweenUnits,
                (delay, spawnVfx, spawnUnit) => StartCoroutine(EnemySpawnCoroutine(delay, spawnVfx, spawnUnit))));
            _spawnCrt = null;
        }

        private IEnumerator EnemySpawnCoroutine(float delay, GameObject spawnVfx, Func<Unit> spawnUnit) {
            yield return new WaitForSeconds(delay);
            Destroy(spawnVfx);
            spawnUnit();
        }

        public void HandleWavesCleared() {
            if (_arenaManager == null) {
                _arenaManager = FindObjectOfType<ArenaManager>();
            }
            
            if (owningPlayer.ControlType == ControlType.Ai) {
                _arenaManager.WavesCleared();
            }
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