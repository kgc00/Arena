using System.Collections.Generic;
using System.Globalization;
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
        public int TimeTaken => Mathf.RoundToInt(Time.time);
        public int Score => (GoldSpent + EnemiesKilled) * TimeTaken;

        void Start() {
            Unit.OnDeath += HandleUnitDeath;
            this.AddObserver(HandlePurchaseComplete, NotificationType.PurchaseComplete);
        }

        private void OnDestroy() {
            Unit.OnDeath -= HandleUnitDeath;
            this.RemoveObserver(HandlePurchaseComplete, NotificationType.PurchaseComplete);
        }

        public void SaveScore() {
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