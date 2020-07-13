using System;
using System.Linq;
using Common;
using Data.Modifiers;
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

        public void AddUnitModifier(UnitSpawnData unitSpawnData, WaveSpawnData waveSpawnData, UnitModifier mod) {
            print("incoming request");
            if (!FindDataStructure(unitSpawnData, waveSpawnData, mod, out var selectedUnit)) return;

            // TODO: Store typeof Modifier, use an activator when loading later to create the instances
            selectedUnit.modifiers.ForEach(m => print($"Type: {m.GetType()}.  AssetPath: {m.IconAssetPath()}"));

            if (DoesExist(mod, selectedUnit)) {
                print($"Modifier already exists.  List length: {selectedUnit.modifiers.Count}");
            } else {
                selectedUnit.modifiers.Add(mod);
                print($"Selected unit's modifier count: {selectedUnit.modifiers.Count}");
            }
        }


        public void RemoveUnitModifier(UnitSpawnData unitSpawnData, WaveSpawnData waveSpawnData, UnitModifier mod) {
            if (!FindDataStructure(unitSpawnData, waveSpawnData, mod, out var selectedUnit)) return;

            // Handles case where modifier was already added
            // and new instance of same type is being supplied by input
            var m = GetModifier(mod, selectedUnit);
            if (m != null) selectedUnit.modifiers.Remove(m);
            
            print($"Selected unit's modifier count: {selectedUnit.modifiers.Count}");
        }


        private bool FindDataStructure(UnitSpawnData unitSpawnData, WaveSpawnData waveSpawnData, UnitModifier mod,
            out UnitSpawnData selectedUnit) {
            selectedUnit = null;

            var selectedWave = Model.Waves.FirstOrDefault(w => waveSpawnData.number == w.number);
            if (selectedWave == null) return false;

            selectedUnit = selectedWave.wave.FirstOrDefault(u => u.Unit == unitSpawnData.Unit);
            if (selectedUnit == null) return false;

            return true;
        }

        private bool DoesExist(UnitModifier mod, UnitSpawnData selectedUnit) =>
            selectedUnit.modifiers.Exists(m => m.GetType() == mod.GetType());
        
        private UnitModifier GetModifier(UnitModifier mod, UnitSpawnData selectedUnit) =>
            selectedUnit.modifiers.FirstOrDefault(m => m.GetType() == mod.GetType());

    }
}