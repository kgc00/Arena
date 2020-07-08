using System;
using Common;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public class Visualizer : MonoBehaviour, IInitializable<HordeSpawnData, Visualizer> {
        public HordeSpawnData model;
        public HordeSpawnData Model {
            get => model;
            set => model = value;
        }

        private VisualizerHeader visualizerHeader;
        private WaveWrapper waveWrapper;
        public bool Initialized { get; private set; }

        
        private void Awake() {
            visualizerHeader = GetComponentInChildren<VisualizerHeader>() ??
                               throw new Exception($"Unable to get VisualizerHeader component in {name}");
            
            waveWrapper = GetComponentInChildren<WaveWrapper>() ??
                               throw new Exception($"Unable to get WaveWrapper component in {name}");

            Initialize(Model);
        }

        public Visualizer Initialize(HordeSpawnData st) {
            Model = st.CreateInstance();
            visualizerHeader.Initialize(Model);
            waveWrapper.Initialize(Model.Waves[0]);
            visualizerHeader.UpdateList();
            waveWrapper.UpdateList();
            Initialized = true;
            return this;
        }
    }
}