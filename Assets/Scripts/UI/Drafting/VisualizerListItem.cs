using System;
using System.Collections.Generic;
using Common;
using Data.Modifiers;
using Data.SpawnData;
using Modifiers.SpawnModifiers;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace UI.Drafting {
    public class VisualizerListItem : MonoBehaviour,
        IInitializable<UnitSpawnData, ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem>, VisualizerListItem
        > {
        public TextMeshProUGUI TypeUgui { get; set; }
        private string typeText;
        private string amountText;
        public TextMeshProUGUI AmountUgui { get; set; }
        public ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem> Owner { get; private set; }
        public UnitSpawnData Model { get; set; }
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject buttonParent;
        private List<GameObject> buttonInstances;

        [NonSerialized, OdinSerialize] private List<UnitModifier> modifiers;

        public VisualizerListItem Initialize(UnitSpawnData m,
            ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem> o) {
            Owner = o;
            Model = m.CreateInstance();
            typeText = m.Unit.ToString();
            amountText = m.Amount.ToString();
            TypeUgui.SetText(typeText);
            AmountUgui.SetText(amountText);

            // TODO: do not hard code modifiers
            modifiers = new List<UnitModifier> {new DoubleUnitEnduranceModifier(), new DoubleUnitStrengthModifier()};
            InitializeModifierButtons();

            Initialized = true;
            return this;
        }

        private void InitializeModifierButtons() {
            buttonInstances = new List<GameObject> {
                Instantiate(buttonPrefab, buttonParent.transform),
                Instantiate(buttonPrefab, buttonParent.transform)
            };

            buttonInstances[0].GetComponent<UnitModifierButton>()
                .Initialize(Model, modifiers[0], Owner as WaveVisualizerWrapper);
            buttonInstances[1].GetComponent<UnitModifierButton>()
                .Initialize(Model, modifiers[1], Owner as WaveVisualizerWrapper);
        }

        public bool Initialized { get; private set; }

        private void Awake() {
            TypeUgui = transform.FindDeepChild("Type").GetComponent<TextMeshProUGUI>() ??
                       throw new Exception("Unable to find type UI element");

            AmountUgui = transform.FindDeepChild("Amount").GetComponent<TextMeshProUGUI>() ??
                         throw new Exception("Unable to find type UI element");
        }
    }
}