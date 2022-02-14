using Common;
using Common.Levels;
using Common.Saving;
using TMPro;
using UnityEngine;

namespace UI {
    public class WinScreen : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _goldSpentTMP;
        [SerializeField] private TextMeshProUGUI _enemiesKilledTMP;
        [SerializeField] private TextMeshProUGUI _timeTakenTMP;
        [SerializeField] private TextMeshProUGUI _scoreTMP;
        private void OnEnable() {
            if (FileManager.LoadFromFile(Constants.SavePath, out var scoreDataString)) {
                var scoreData = new ScoreData().LoadFromJson(scoreDataString);

                _goldSpentTMP.text = scoreData.goldSpent.ToString();
                _enemiesKilledTMP.text = scoreData.enemiesKilled.ToString();
                _timeTakenTMP.text = scoreData.timeTaken.ToString();
                _scoreTMP.text = scoreData.score.ToString();
            }
        }

        public void HandleContinue() {
            LevelDirector.Instance.LoadMain();
        }

        public void HandleShare() {
            // todo
        }
    }
}