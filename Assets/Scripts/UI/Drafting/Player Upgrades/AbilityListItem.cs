using System.Collections.Generic;
using Abilities.Modifiers;
using Data.SpawnData;
using TMPro;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilityListItem : MonoBehaviour {
        public IModifierHandler<UnitSpawnData, AbilityModifier> Owner { get; private set; }
        public UnitSpawnData Model { get; private set; }

        public bool Initialized { get; private set; }

        [SerializeField] private GameObject buttonPrefab;

        [SerializeField] private GameObject buttonParent;

        private List<GameObject> buttonInstances;
        [SerializeField] private TextMeshProUGUI textUgui;
        public AbilityListItem Initialize(UnitSpawnData m, AbilityModifier mod,
            IModifierHandler<UnitSpawnData, AbilityModifier> o) {

            Owner = o;
            Model = m.CreateInstance();
            // textUgui.SetText($"{stat.Type.ToString()}:");
            // modifiers = ModifiersFromStatType(stat);
            // InitializeModifierButtons();
            Initialized = true;
            return this;
        }
    }
}