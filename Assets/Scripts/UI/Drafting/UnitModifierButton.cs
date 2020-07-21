using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.UnitData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TypeReferences;

namespace UI.Drafting {
    [RequireComponent(typeof(Toggle))]
    public sealed class UnitModifierButton : MonoBehaviour {
        public IModifierHandler<UnitSpawnData, UnitModifier> Owner { get; protected set; }
        public UnitSpawnData SpawnModel { get; protected set; }
        public UnitData UnitModel { get; protected set; }
        public UnitModifier Modifier { get; protected set; }
        public bool Initialized { get; protected set; }
        [SerializeField] public Image iconImage;
        [SerializeField] public Image buttonImage;
        private Toggle toggle;
        private Color[] buttonColors;
        
        [SerializeField] private TextMeshProUGUI textUgui;

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

        public UnitModifierButton Initialize(UnitSpawnData model, UnitModifier mod, IModifierHandler<UnitSpawnData, UnitModifier> o) {
            Owner = o;
            Modifier = mod;
            SpawnModel = model;
            UnitModel = DataHelper.DataFromUnitType(model.Unit);
            iconImage.sprite = Resources.Load<Sprite>(Modifier.IconAssetPath());
            textUgui.SetText(Modifier.DisplayText());
            SetInitialToggleState();
            Initialized = true;
            return this;
        }

        private void SetInitialToggleState() {
            if (SpawnModel.modifiers.Count == 0) return;
            
            SpawnModel.modifiers.ForEach(m => {
                if (m == Modifier.GetType()) toggle.isOn = true;
            });
        }
    }
}