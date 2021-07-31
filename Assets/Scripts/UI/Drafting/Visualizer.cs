using System;
using System.Linq;
using Common;
using Common.Levels;
using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.Types;
using Sirenix.Serialization;
using UnityEngine;

namespace UI.Drafting {
    public class Visualizer : MonoBehaviour, IInitializable<HordeSpawnData, Visualizer, Visualizer> {
        [OdinSerialize] public HordeSpawnData model;
        public Visualizer Owner { get; private set; }

        public HordeSpawnData Model {
            get => model;
            set => model = value;
        }

        private VisualizerHeader _visualizerHeader;
        private WaveVisualizerWrapper _waveVisualizerWrapper;
        public bool Initialized { get; private set; }
        
        private void Awake() {
            _visualizerHeader = GetComponentInChildren<VisualizerHeader>() ??
                               throw new Exception($"Unable to get VisualizerHeader component in {name}");

            _waveVisualizerWrapper = GetComponentInChildren<WaveVisualizerWrapper>() ??
                                    throw new Exception($"Unable to get WaveWrapper component in {name}");

        }

        private void Start() {
            Initialize(Model, this);
            UpdateUI();
        }

        private void UpdateUI() {
            _waveVisualizerWrapper.UpdateModel(Model.Waves[0]);
            _visualizerHeader.UpdateList();
            _waveVisualizerWrapper.UpdateList();
        }

        private void OnEnable() {
            WaveButton.OnWaveButtonClick += UpdateVisualizerList;
            if (Initialized) {
                UpdateUI();
            }
        }

        private void OnDisable() {
            WaveButton.OnWaveButtonClick -= UpdateVisualizerList;
            Initialized = false;
        }

        public Visualizer Initialize(HordeSpawnData m, Visualizer o) {
            Owner = o;
            Model = m ? m.CreateInstance() : PersistentData.Instance.CurrentHordeModel[ControlType.Ai].CreateInstance();
            Model.AssignWaveNumbers();
            _visualizerHeader.Initialize(Model, this);
            _waveVisualizerWrapper.Initialize(Model.Waves[0], this);
            _visualizerHeader.UpdateList();
            _waveVisualizerWrapper.UpdateList();
            Initialized = true;
            return this;
        }

        public void UpdateVisualizerList(int i) => _waveVisualizerWrapper.UpdateModel(Model.Waves[i]);

        public void AddUnitModifier(UnitSpawnData unitSpawnData, WaveSpawnData waveSpawnData, UnitModifierType mod,
            int cost) {
            if (!FindDataStructure(unitSpawnData, waveSpawnData, out var selectedUnit)) return;

            if (DoesExist(mod, selectedUnit)) return;
            selectedUnit.modifiers.Add(mod);
            PersistentData.Instance.currency += cost;
        }


        public void RemoveUnitModifier(UnitSpawnData unitSpawnData, WaveSpawnData waveSpawnData, UnitModifierType mod,
            int cost) {
            if (!FindDataStructure(unitSpawnData, waveSpawnData, out var selectedUnit)) return;

            // Handles case where modifier was already added
            // and new instance of same type is being supplied by input
            var m = GetUnitModifierType(mod, selectedUnit);
            selectedUnit.modifiers.Remove(m);
            PersistentData.Instance.currency -= cost;
        }
        
        private bool FindDataStructure(UnitSpawnData unitSpawnData, WaveSpawnData waveSpawnData, out UnitSpawnData selectedUnit) {
            selectedUnit = null;

            var selectedWave = Model.Waves.FirstOrDefault(w => waveSpawnData.number == w.number);
            if (selectedWave == null) return false;

            selectedUnit = selectedWave.wave.FirstOrDefault(u => u.Unit == unitSpawnData.Unit);
            return selectedUnit != null;
        }

        private bool DoesExist(UnitModifierType mod, UnitSpawnData selectedUnit) =>
            selectedUnit.modifiers.Exists(m => m == mod);
        
        private UnitModifierType GetUnitModifierType(UnitModifierType mod, UnitSpawnData selectedUnit) =>
            selectedUnit.modifiers.FirstOrDefault(m => m == mod);

        public void HandleContinue() {
            PersistentData.Instance.UpdateHordeModel(ControlType.Ai, Model);
            LevelDirector.Instance.LoadArena();
        }
    }
}