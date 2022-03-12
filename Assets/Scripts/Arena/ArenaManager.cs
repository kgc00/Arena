using System.Collections;
using System.Linq;
using Audio;
using Common.Levels;
using Controls;
using Data;
using Data.Types;
using Pooling;
using Sirenix.Utilities;
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

            AudioService.Instance.RequestBGM();
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
            ReturnAllObjectsToPool();
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
                yield return StartCoroutine(HandleWin());
            }
            else {
                yield return StartCoroutine(HandleWaveClearedLogic());
            }
        }

        private IEnumerator HandleWaveClearedLogic() {
            if (_playerController == null) {
                _playerController = FindObjectOfType<PlayerController>();
            }

            if (_spawnManager == null) {
                _spawnManager = FindObjectsOfType<SpawnManager>()
                    .FirstOrDefault(x => x.owningPlayer.ControlType == ControlType.Ai);
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

        public IEnumerator HandleWin() {
            if (_scoreKeeper == null) {
                _scoreKeeper = FindObjectOfType<ScoreKeeper>();
            }
            _scoreKeeper.SaveScore();
            AudioService.Instance.RequestFadeOutBGM();
            yield return new WaitForSeconds(1);
            ReturnAllObjectsToPool();
            yield return new WaitForEndOfFrame();
            LevelDirector.Instance.LoadWin();
        }

        private void ReturnAllObjectsToPool() {
            FindObjectsOfType<Unit>(false).ForEach(x => ObjectPool.AddOrReturnInstanceToPool(x.poolKey, x));
        }
    }
}