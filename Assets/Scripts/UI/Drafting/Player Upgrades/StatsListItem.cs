using System.Collections.Generic;
using Common;
using Data.Modifiers;
using Data.SpawnData;
using Data.Stats;
using Data.Types;
using Data.UnitData;
using Modifiers.SpawnModifiers;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class StatsListItem : MonoBehaviour, IInitializable<UnitSpawnData, ModeledList<UnitSpawnData, UnitSpawnData, StatsListItem>, StatsListItem> {
        public ModeledList<UnitSpawnData, UnitSpawnData, StatsListItem> Owner { get; }
        public UnitSpawnData Model { get; }
        private List<UnitModifier> modifiers;
        private StatType statType;
        
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject buttonParent;
        private List<GameObject> buttonInstances;
        
        public StatsListItem Initialize(UnitSpawnData m, ModeledList<UnitSpawnData, UnitSpawnData, StatsListItem> o) {
            Owner = o;
            Model = m.CreateInstance();
            modifiers = ModifiersFromStatType(statType);
            return this;
        }

        private List<UnitModifier> ModifiersFromStatType(StatType type) {
            switch (type) {
                case StatType.MovementSpeed:
                    return new List<UnitModifier> { new UnitMovementSpeedIncreaseSmallModifier(), new UnitMovementSpeedIncreaseMediumModifier(), new DoubleUnitMovementSpeedModifier()};
                default:
                    return new List<UnitModifier>();
            }
        }

        
        private void InitializeModifierButtons() {
            buttonInstances = new List<GameObject> {
                Instantiate(buttonPrefab, buttonParent.transform),
                Instantiate(buttonPrefab, buttonParent.transform)
            };

            // buttonInstances[0].GetComponent<UnitModifierButton>()
            //     .Initialize(Model, modifiers[0], Owner as StatsUpgradesList);
            // buttonInstances[1].GetComponent<UnitModifierButton>()
            //     .Initialize(Model, modifiers[1], Owner as StatsUpgradesList);
        }

        
        public bool Initialized { get; }
    }
}