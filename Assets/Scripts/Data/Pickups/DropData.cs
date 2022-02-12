using System;
using UnityEngine;

namespace Data.Pickups {
    [Serializable]
    public class DropData {
        public DropType dropType;
        public float dropRate;
        public DropData(DropType dropType, float dropRate) {
            this.dropType = dropType;
            this.dropRate = Mathf.Clamp(dropRate, 0, 100);
        }
    }
}