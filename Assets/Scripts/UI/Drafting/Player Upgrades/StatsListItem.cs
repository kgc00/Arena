﻿using System;
using System.Collections.Generic;
using Data.Modifiers;
using Data.SpawnData;
using Data.Stats;
using Data.Types;
using Modifiers.SpawnModifiers;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class StatsListItem : MonoBehaviour {
        public bool Initialized { get; private set; }
        public IModifierHandler<UnitSpawnData, UnitModifier> Owner { get; set; }
        public UnitSpawnData Model { get; private set; }
        [NonSerialized, OdinSerialize] private List<UnitModifier> modifiers;
        private Statistic stat;

        [SerializeField] private GameObject buttonPrefab;

        [SerializeField] private GameObject buttonParent;

        private List<GameObject> buttonInstances;
        [SerializeField] private TextMeshProUGUI textUgui;

        public StatsListItem Initialize(UnitSpawnData m, Statistic s, IModifierHandler<UnitSpawnData, UnitModifier> o) {
            Owner = o;
            Model = m.CreateInstance();
            stat = s;
            textUgui.SetText($"{stat.Type.ToString()}:");
            modifiers = ModifiersFromStatType(stat);
            InitializeModifierButtons();
            Initialized = true;
            return this;
        }

        private List<UnitModifier> ModifiersFromStatType(Statistic s) {
            switch (s.Type) {
                case StatType.MovementSpeed:
                    return new List<UnitModifier> {
                        new UnitMovementSpeedIncreaseSmallModifier(),
                        new UnitMovementSpeedIncreaseMediumModifier(),
                        new DoubleUnitMovementSpeedModifier()
                    };
                case StatType.Strength:
                    return new List<UnitModifier> {
                        new UnitStrengthIncreaseSmallModifier(),
                        new UnitStrengthIncreaseMediumModifier(),
                        new DoubleUnitStrengthModifier()
                    };
                case StatType.Endurance:
                    return new List<UnitModifier> {
                        new UnitEnduranceIncreaseSmallModifier(),
                        new UnitEnduranceIncreaseMediumModifier(),
                        new DoubleUnitEnduranceModifier()
                    };
                default:
                    return new List<UnitModifier>();
            }
        }


        private void InitializeModifierButtons() {
            buttonInstances = new List<GameObject>();
            foreach (var mod in modifiers) {
                var go = Instantiate(buttonPrefab, buttonParent.transform);
                buttonInstances.Add(go);
                go.GetComponent<UnitModifierButton>()
                    .Initialize(Model, mod, Owner);
            }
        }
    }
}