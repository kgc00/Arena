using System;
using Common;
using Data.SpawnData;
using UnityEngine;

namespace UI.Drafting {
    public class Visualizer : MonoBehaviour, IInitializable<HordeSpawnData, Visualizer, Visualizer> {
        public HordeSpawnData model;
        public Visualizer Owner { get; private set; }

        public HordeSpawnData Model {
            get => model;
            set => model = value;
        }

        private VisualizerHeader visualizerHeader;
        private WaveVisualizerWrapper waveVisualizerWrapper;
        public bool Initialized { get; private set; }

        
        private void Awake() {
            visualizerHeader = GetComponentInChildren<VisualizerHeader>() ??
                               throw new Exception($"Unable to get VisualizerHeader component in {name}");
            
            waveVisualizerWrapper = GetComponentInChildren<WaveVisualizerWrapper>() ??
                               throw new Exception($"Unable to get WaveWrapper component in {name}");

            Initialize(Model, this);
        }

        private void OnEnable() {
            WaveButton.OnWaveButtonClick += UpdateVisualizerList;
            waveVisualizerWrapper.UpdateModel(Model.Waves[0]);
            visualizerHeader.UpdateList();
            waveVisualizerWrapper.UpdateList();
        }

        private void OnDisable() {
            WaveButton.OnWaveButtonClick -= UpdateVisualizerList;
            Initialized = false;
        }

        public Visualizer Initialize(HordeSpawnData m, Visualizer o) {
            Owner = o;
            Model = m.CreateInstance();
            Model.AssignWaveNumbers();
            visualizerHeader.Initialize(Model, this);
            waveVisualizerWrapper.Initialize(Model.Waves[0], this);
            visualizerHeader.UpdateList();
            waveVisualizerWrapper.UpdateList();
            Initialized = true;
            return this;
        }

        public void UpdateVisualizerList(int i) => waveVisualizerWrapper.UpdateModel(Model.Waves[i]);
    }
}