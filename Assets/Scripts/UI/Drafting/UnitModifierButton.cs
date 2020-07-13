using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.UnitData;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace UI.Drafting {
    [RequireComponent(typeof(Toggle))]
    public class UnitModifierButton : MonoBehaviour {
        public WaveVisualizerWrapper Owner { get; protected set; }
        public UnitSpawnData SpawnModel { get; protected set; }
        public UnitData UnitModel { get; protected set; }
        public UnitModifier Modifier { get; protected set; }
        public bool Initialized { get; protected set; }
        [SerializeField] public Image iconImage;
        [SerializeField] public Image buttonImage;
        private Toggle toggle;
        private Color[] buttonColors;

        private void Awake() {
            Debug.Assert(iconImage != null, nameof(iconImage) + " != null");

            buttonColors = new[] {Color.white, Color.green};

            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
        }

        private void ToggleValueChanged(Toggle change) {
            if (change.isOn) {
                Owner.AddModifier(SpawnModel, Modifier);
                buttonImage.color = buttonColors[1];
            }
            else {
                Owner.RemoveModifier(SpawnModel, Modifier);
                buttonImage.color = buttonColors[0];
            }
        }

        // TODO: Map from UnitSpawnData to UnitData
        public UnitModifierButton Initialize(UnitSpawnData model, UnitModifier mod, WaveVisualizerWrapper o) {
            Owner = o;
            Modifier = mod;
            SpawnModel = model;
            UnitModel = DataHelper.DataFromUnitType(model.Unit).CreateInstance();
            iconImage.sprite = Resources.Load<Sprite>(Modifier.IconAssetPath());
            Initialized = true;
            SetInitialToggleState();
            return this;
        }

        private void SetInitialToggleState() {
            if (SpawnModel.modifiers.Count == 0) return;
            
            SpawnModel.modifiers.ForEach(m => {
                if (m.GetType() == Modifier.GetType()) toggle.isOn = true;
            });
        }
    }
}