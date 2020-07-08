using System;
using System.Collections.Generic;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public class VisualizerHeader : ModeledList<HordeSpawnData, WaveSpawnData, WaveButton> {
        protected override List<WaveSpawnData> Map(HordeSpawnData model) => model.Waves;
    }
}