using System;
using Common;
using Data.SpawnData;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Drafting {
    public class VisualizerListItem : MonoBehaviour,
        IInitializable<UnitSpawnData, ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem>, VisualizerListItem> {
        public TextMeshProUGUI TypeUgui { get; set; }
        private string typeText;
        private string amountText;
        public TextMeshProUGUI AmountUgui { get; set; }
        public ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem> Owner { get; private set; }
        public UnitSpawnData Model { get; set; }

        public VisualizerListItem Initialize(UnitSpawnData m,
            ModeledList<WaveSpawnData, UnitSpawnData, VisualizerListItem> o) {
            Owner = o;
            Model = m;
            typeText = m.Unit.ToString();
            amountText = m.Amount.ToString();
            TypeUgui.SetText(typeText);
            AmountUgui.SetText(amountText);
            Initialized = true;
            return this;
        }

        public bool Initialized { get; private set; }

        private void Awake() {
            TypeUgui = transform.Find("Type").GetComponent<TextMeshProUGUI>() ??
                       throw new Exception("Unable to find type UI element");

            AmountUgui = transform.Find("Amount").GetComponent<TextMeshProUGUI>() ??
                         throw new Exception("Unable to find type UI element");
        }
    }
}