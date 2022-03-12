using Common;
using Common.Saving;
using Data.Types;
using UI.InGameShop;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI {
    public class ScoreKeeper : MonoBehaviour {
        public int EnemiesKilled;
        public int GoldSpent;
        public int TimeTaken => Mathf.RoundToInt(Time.time) - _startTime;
        private int ScoreWithoutPenalty => GoldSpent + EnemiesKilled * 100 + TimeTaken;
        public int Score => Mathf.Max(_didDie ? ScoreWithoutPenalty / 2 : ScoreWithoutPenalty, 0);
        private bool _didDie;
        private int _startTime;

        void Start() {
            Unit.OnDeath += HandleUnitDeath;
            this.AddObserver(HandlePurchaseComplete, NotificationType.PurchaseComplete);
            _startTime = Mathf.RoundToInt(Time.time);
        }

        private void OnDestroy() {
            Unit.OnDeath -= HandleUnitDeath;
            this.RemoveObserver(HandlePurchaseComplete, NotificationType.PurchaseComplete);
        }

        public void SaveScore(bool didDie = false) {
            _didDie = didDie;
            var scoreData = new ScoreData {
                score = Score, enemiesKilled = EnemiesKilled, timeTaken = TimeTaken, goldSpent = GoldSpent
            };
            FileManager.WriteToFile(Constants.SavePath, scoreData.ToJson());
        }

        private void HandlePurchaseComplete(object sender, object args) {
            if (args == null || !(args is PurchaseEvent purchaseEvent)) return;
            GoldSpent += purchaseEvent.Cost;
        }

        private void HandleUnitDeath(Unit unit) {
            if (unit.Owner.ControlType != ControlType.Ai) return;
            EnemiesKilled++;
        }
    }
}