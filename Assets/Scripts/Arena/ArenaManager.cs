using System.Collections;
using Common;
using Common.Levels;
using Data;
using Data.Types;
using UnityEngine;

namespace Arena {
    public class ArenaManager : MonoBehaviour {
        [SerializeField] private float delayBeforeLoad;

        public void WavesCleared() {
            StartCoroutine(HandleWavesCleared());
        }

        private IEnumerator HandleWavesCleared() {
            yield return new WaitForSeconds(delayBeforeLoad);

            var wasFinalHorde = PersistentData.Instance.EnemyHordes.Count - 1 <= PersistentData.Instance.CurIndex;
            if (wasFinalHorde) {
                // TODO: implement win screen
                print("You win!");
            }
            else {
                PersistentData.Instance.IncrementHordeModel();
                LevelDirector.Instance.LoadUpgrades();
            }
        }
    }
}