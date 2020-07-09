using System;
using System.Collections.Generic;
using Common;
using Data.SpawnData;
using UnityEngine;

namespace UI.Drafting {
    public class WaveVisualizerWrapper : ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem>,
        IInitializable<WaveSpawnData, Visualizer, WaveVisualizerWrapper> {
        protected override List<UnitSpawnData> Map(WaveSpawnData model) => model.wave;
        public Visualizer Owner { get; private set; }

        public WaveVisualizerWrapper Initialize(WaveSpawnData m, Visualizer o) {
            Owner = o;
            Model = m;
            Initialized = true;
            return this;
        }
    }
}