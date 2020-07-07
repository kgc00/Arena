using System;
using Common;
using Spawner.Data;
using TMPro;
using UnityEngine;

namespace UI.Drafting {
    public class VisualizerListItem : MonoBehaviour, IInitializable<UnitSpawnData, VisualizerListItem> {
        public TextMeshProUGUI TypeUgui {get; set; }
        private string typeText;
        private string amountText;
        public TextMeshProUGUI AmountUgui { get; set; }
        public UnitSpawnData Model { get; set; }
        private void Awake() {
            TypeUgui = transform.Find("Type").GetComponent<TextMeshProUGUI>() ??
                   throw new Exception("Unable to find type UI element");
            
            AmountUgui = transform.Find("Amount").GetComponent<TextMeshProUGUI>() ??
                     throw new Exception("Unable to find type UI element");
        }

        public VisualizerListItem Initialize(UnitSpawnData ut) {
            Model = ut;
            typeText = ut.Unit.ToString();
            amountText = ut.Amount.ToString();
            TypeUgui.SetText(typeText);
            AmountUgui.SetText(amountText);
            return this;
        }
    }
}