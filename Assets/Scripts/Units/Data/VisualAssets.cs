using System;
using UnityEngine;

namespace Units.Data {
    [Serializable]
    public class VisualAssets {
        [SerializeField] public Sprite portrait;
        public VisualAssets(VisualAssets data) {
            this.portrait = data.portrait;
        }
    }
}