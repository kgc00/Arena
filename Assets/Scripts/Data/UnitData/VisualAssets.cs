using System;
using UnityEngine;

namespace Data.UnitData {
    [Serializable]
    public class VisualAssets {
        [SerializeField] public Sprite portrait;
        public VisualAssets(VisualAssets data) {
            this.portrait = data.portrait;
        }
    }
}