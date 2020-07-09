using System.Collections.Generic;
using Common;
using Data.SpawnData;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Drafting {
    public class VisualizerHeader : ModeledList<HordeSpawnData, WaveSpawnData, WaveButton>, 
        IInitializable<HordeSpawnData, Visualizer, VisualizerHeader> {
        protected override List<WaveSpawnData> Map(HordeSpawnData model) => model.Waves;
        [FormerlySerializedAs("ScrollContent"),SerializeField] private GameObject scrollContent;
        public Visualizer Owner { get; private set; }
        public VisualizerHeader Initialize(HordeSpawnData m, Visualizer o) {
            Model = m;
            Owner = o;
            Initialized = true;
            return this;
        }

        protected override void AddListItem(WaveSpawnData data) =>
            ListItems.Add(Instantiate(listItem, scrollContent.transform).GetComponent<WaveButton>().Initialize(data, this));
    }
}