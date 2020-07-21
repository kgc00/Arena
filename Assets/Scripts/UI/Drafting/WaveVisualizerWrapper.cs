﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Modifiers;
using Data.SpawnData;
using Data.UnitData;
using TMPro;
using UnityEngine;

namespace UI.Drafting {
    public class WaveVisualizerWrapper : ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem>,
        IInitializable<WaveSpawnData, Visualizer, WaveVisualizerWrapper> {
        protected override List<UnitSpawnData> Map(WaveSpawnData model) => model.wave;
        public Visualizer Owner { get; private set; }

        private TextMeshProUGUI waveTextUgui;
        [SerializeField] private GameObject waveText;
        [SerializeField] private GameObject preferredParent;

        private void Awake() {
            waveTextUgui = waveText.GetComponent<TextMeshProUGUI>() ??
                           throw new Exception($"Unable to get TextMeshProUGUI component in {name}");
        }

        public WaveVisualizerWrapper Initialize(WaveSpawnData m, Visualizer o) {
            Owner = o;
            Model = m.CreateInstance();
            Initialized = true;
            return this;
        }

        public override void UpdateModel(WaveSpawnData m) {
            base.UpdateModel(m);
            waveTextUgui.SetText($"Wave {Model.number + 1}");
        }
        
        
        protected override void CreateList(GameObject p = null) => base.CreateList(preferredParent);

        public void AddModifier(UnitSpawnData spawnModel, UnitModifier modifier) {
            Owner.AddUnitModifier(spawnModel, Model, modifier);
        }

        public void RemoveModifier(UnitSpawnData spawnModel, UnitModifier modifier) {
            Owner.RemoveUnitModifier(spawnModel, Model, modifier);
        }
    }
}