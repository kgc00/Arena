using System.Collections;
using Audio;
using Common;
using Common.Levels;
using Common.Saving;
using Data.Types;
using TMPro;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI {
    public class WinScreen : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _goldSpentTMP;
        [SerializeField] private TextMeshProUGUI _enemiesKilledTMP;
        [SerializeField] private TextMeshProUGUI _timeTakenTMP;
        [SerializeField] private TextMeshProUGUI _scoreTMP;

        private void Start() {
            StartCoroutine(HandleAudio());
            if (FileManager.LoadFromFile(Constants.SavePath, out var scoreDataString)) {
                var scoreData = new ScoreData().LoadFromJson(scoreDataString);

                _goldSpentTMP.text = scoreData.goldSpent.ToString();
                _enemiesKilledTMP.text = scoreData.enemiesKilled.ToString();
                _timeTakenTMP.text = scoreData.timeTaken.ToString();
                _scoreTMP.text = scoreData.score.ToString();
            }
        }

        private IEnumerator HandleAudio() {
            this.PostNotification(NotificationType.DidWin);
            yield return new WaitForSeconds(4);
            AudioService.Instance.RequestBGM();
        }

        public void HandleContinue() {
            this.PostNotification(NotificationType.DidClickShopButton);
            LevelDirector.Instance.LoadMain();
        }

        public void HandleShare() {
            // todo
        }
    }
}