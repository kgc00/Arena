﻿using System.Collections;
using Common.Levels;
using Controls;
using Data;
using Data.Types;
using Spawner;
using UI;
using UI.InGameShop;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace Arena {
    public class ArenaManager : MonoBehaviour {
        [SerializeField] private float delayBeforeLoad;
        private PlayerController _playerController;
        private SpawnManager _spawnManager;
        private ArenaData _arenaData;
        private InGameShopManager _inGameShopManager;
        private ScoreKeeper _scoreKeeper;

        private void Start() {
            Unit.OnDeath += HandleUnitDeath;
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
        }

        private void OnDestroy() {
            Unit.OnDeath -= HandleUnitDeath;
        }

        private void HandleUnitDeath(Unit unit) {
            if (unit.Owner.ControlType != ControlType.Local) return;
            if (_scoreKeeper == null) {
                _scoreKeeper = FindObjectOfType<ScoreKeeper>();
            }
            StartCoroutine(HandleLoseCrt());
        }

        private IEnumerator HandleLoseCrt() {
            this.PostNotification(NotificationType.GameOver);
            _scoreKeeper.SaveScore(true);
            yield return new WaitForSeconds(4f);
            LevelDirector.Instance.LoadLose();
        }

        public void WavesCleared() {
            StartCoroutine(HandleWavesCleared());
        }

        private IEnumerator HandleWavesCleared() {
            if (_arenaData == null) {
                _arenaData = FindObjectOfType<ArenaData>();
            }

            var wasFinalHorde = _arenaData.EnemyWaves.Count - 1 == _arenaData.CurIndex;
            this.PostNotification(NotificationType.WaveCleared);
            yield return new WaitForSeconds(delayBeforeLoad);
            if (wasFinalHorde) {
                if (_scoreKeeper == null) {
                    _scoreKeeper = FindObjectOfType<ScoreKeeper>();
                }
                _scoreKeeper.SaveScore();
                LevelDirector.Instance.LoadWin();
            }
            else {
                if (_playerController == null) {
                    _playerController = FindObjectOfType<PlayerController>();
                }

                if (_spawnManager == null) {
                    _spawnManager = FindObjectOfType<SpawnManager>();
                }

                Debug.Assert(_playerController != null);
                Debug.Assert(_spawnManager != null);
                _playerController.EnableUISchema();
                _inGameShopManager.ToggleVisibility();
                yield return new WaitUntil(() => !_inGameShopManager.isShopVisible);
                _playerController.EnablePlayerSchema();
                _arenaData.IncrementWaveModel();
                _spawnManager.StartSpawn(_arenaData.CurrentWaveModel[ControlType.Ai]);
            }
        }
    }
}