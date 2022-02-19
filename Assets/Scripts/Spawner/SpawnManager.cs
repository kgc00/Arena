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
using Utils.NotificationCenter;

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
        private Coroutine _checkWavesClearedCRT;

        private void OnEnable() {
            if (owningPlayer.ControlType == ControlType.Ai) {
                Unit.OnDeath += HandleUnitDeath;
            }
        }

        private void OnDisable() {
            if (owningPlayer.ControlType == ControlType.Ai) {
                Unit.OnDeath -= HandleUnitDeath;
            }
        }

        private void Start() {
            transform.position = new Vector3(xPos, 0, zPos);
            Bounds = new Vector3(size + xModifier, .1f, size + zModifier);

            WaveSpawner ??= new WaveSpawner(this);
            waveSpawnData = FindObjectOfType<ArenaData>().CurrentWaveModel[owningPlayer.ControlType];

            StartSpawn(waveSpawnData);
        }

        private IEnumerator CheckWaveClearedCRT() {
            yield return new WaitForSeconds(1.5f);
            if (owningPlayer.Units.Count > 0) yield break;
            HandleWavesCleared();
        }

        private void HandleUnitDeath(Unit unit) {
            if (unit.Owner.ControlType != ControlType.Ai || _spawnCrt != null) return;
            if (_checkWavesClearedCRT != null) {
                StopCoroutine(_checkWavesClearedCRT);
            }
            _checkWavesClearedCRT = StartCoroutine(CheckWaveClearedCRT());
        }

        public void StartSpawn(WaveSpawnData spawnData) {
            Debug.Assert(_spawnCrt == null);
            _spawnCrt = StartCoroutine(SpawnWave(spawnData));
        }

        private IEnumerator SpawnWave(WaveSpawnData spawnData) {
            yield return StartCoroutine
            (WaveSpawner.Spawn(spawnData, model.spawnStartupTime,
                model.delayBetweenUnits,
                (delay, spawnVfx, spawnUnit) => StartCoroutine(EnemySpawnCoroutine(delay, spawnVfx, spawnUnit))));
            // yield return new WaitForSeconds(2f);
            _spawnCrt = null;
        }

        private IEnumerator EnemySpawnCoroutine(float delay, GameObject spawnVfx, Func<Unit> spawnUnit) {
            yield return new WaitForSeconds(delay);
            Destroy(spawnVfx);
            var unit = spawnUnit();
            this.PostNotification(NotificationType.UnitDidSpawn, unit);
        }

        public void HandleWavesCleared() {
            if (_arenaManager == null) {
                _arenaManager = FindObjectOfType<ArenaManager>();
            }

            if (owningPlayer.ControlType == ControlType.Ai) {
                _checkWavesClearedCRT = null;
                _arenaManager.WavesCleared();
            }
        }

        private void OnGUI() {
            if (owningPlayer.ControlType == ControlType.Ai)
            GUILayout.Box((_checkWavesClearedCRT == null).ToString());
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