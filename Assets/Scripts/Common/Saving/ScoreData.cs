using UnityEngine;

namespace Common.Saving {
    [System.Serializable]
    public class ScoreData {
        public int score;
        public int enemiesKilled;
        public int goldSpent;
        public int timeTaken;
        
        public string ToJson() {
            return JsonUtility.ToJson(this);
        }

        public ScoreData LoadFromJson(string json) {
            JsonUtility.FromJsonOverwrite(json, this);
            return this;
        }
    }
}