using System.Collections;
using Common.Levels;
using Controls;
using Data;
using Spawner;
using UI;
using UI.InGameShop;
using UnityEngine;

namespace Arena {
    public class ArenaManager : MonoBehaviour {
        [SerializeField] private float delayBeforeLoad;
        private PlayerController _playerController;
        private SpawnManager _spawnManager;
        private ArenaData _arenaData;
        private InGameShopManager _inGameShopManager;

        private void Start() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
        }

        public void WavesCleared() {
            StartCoroutine(HandleWavesCleared());
        }

        private IEnumerator HandleWavesCleared() {
            yield return new WaitForSeconds(delayBeforeLoad);

            if (_arenaData == null) {
                _arenaData = FindObjectOfType<ArenaData>();
            }
            var wasFinalHorde = _arenaData.EnemyWaves.Count - 1 <= _arenaData.CurIndex;
            if (wasFinalHorde) {
                var scoreKeeper = FindObjectOfType<ScoreKeeper>();
                scoreKeeper.SaveScore();
                LevelDirector.Instance.LoadWin();
            }
            else {
                if (_playerController == null)
                {
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
                _spawnManager.StartSpawn();
            }
        }
    }
}