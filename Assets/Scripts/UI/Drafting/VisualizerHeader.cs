using System;
using System.Collections.Generic;
using Common;
using Data.SpawnData;
using Sirenix.Serialization;
using UnityEngine;

namespace UI.Drafting {
    public class VisualizerHeader : ModeledList<HordeSpawnData, WaveSpawnData, WaveButton>, 
        IInitializable<HordeSpawnData, Visualizer, VisualizerHeader> {
        protected override List<WaveSpawnData> Map(HordeSpawnData model) => model.Waves;
        public Visualizer Owner { get; private set; }
        [NonSerialized, OdinSerialize] private GameObject _preferredParent;

        public VisualizerHeader Initialize(HordeSpawnData m, Visualizer o) {
            Model = m;
            Owner = o;
            Initialized = true;
            return this;
        }

        protected override void CreateList(GameObject p = null) => base.CreateList(_preferredParent);
    }
}