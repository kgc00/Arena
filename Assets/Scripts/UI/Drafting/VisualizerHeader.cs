using System.Collections.Generic;
using Common;
using Data.SpawnData;

namespace UI.Drafting {
    public class VisualizerHeader : ModeledList<HordeSpawnData, WaveSpawnData, WaveButton>, 
        IInitializable<HordeSpawnData, Visualizer, VisualizerHeader> {
        protected override List<WaveSpawnData> Map(HordeSpawnData model) => model.Waves;
        public Visualizer Owner { get; private set; }
        public VisualizerHeader Initialize(HordeSpawnData m, Visualizer o) {
            Model = m;
            Owner = o;
            Initialized = true;
            return this;
        }
    }
}