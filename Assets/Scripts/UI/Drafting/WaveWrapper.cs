using System;
using System.Collections.Generic;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public class WaveWrapper : ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem> {
        protected override void CreateList() {
            ListItems = new List<VisualizerListItem>();
            Model.wave.ForEach(AddListItem);
        }
    }
}