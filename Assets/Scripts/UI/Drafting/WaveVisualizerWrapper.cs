using System;
using System.Collections.Generic;
using Common;
using Data.SpawnData;
using TMPro;
using UnityEngine;

namespace UI.Drafting {
    public class WaveVisualizerWrapper : ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem>,
        IInitializable<WaveSpawnData, Visualizer, WaveVisualizerWrapper> {
        protected override List<UnitSpawnData> Map(WaveSpawnData model) => model.wave;
        public Visualizer Owner { get; private set; }

        private TextMeshProUGUI waveTextUgui;
        [SerializeField] private GameObject waveText;

        private void Awake() {
            waveTextUgui = waveText.GetComponent<TextMeshProUGUI>() ??
                           throw new Exception($"Unable to get TextMeshProUGUI component in {name}");
        }

        public WaveVisualizerWrapper Initialize(WaveSpawnData m, Visualizer o) {
            Owner = o;
            Model = m;
            Initialized = true;
            return this;
        }
        
        public override void UpdateModel(WaveSpawnData m) {
            base.UpdateModel(m);
            waveTextUgui.SetText($"Wave {Model.number + 1}");
        }
    }
}