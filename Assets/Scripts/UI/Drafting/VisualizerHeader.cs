using System;
using System.Collections.Generic;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public class VisualizerHeader : ModeledList<HordeSpawnData, WaveSpawnData, WaveButton> {
        protected override void CreateList() {
            ListItems = new List<WaveButton>();
            Model.Waves.ForEach(AddListItem);
        }
    }
}