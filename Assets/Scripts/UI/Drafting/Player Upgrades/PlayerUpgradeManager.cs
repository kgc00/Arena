using System;
using Common;
using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.Types;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class PlayerUpgradeManager : MonoBehaviour,
        IModifierHandler<UnitSpawnData, UnitModifier> {
        public PlayerUpgradeManager Owner { get; private set; }
        private UnitSpawnData model;

        public UnitSpawnData Model {
            get => model;
            set => model = value;
        }

        [SerializeField] private StatsUpgradesList statsUpgradesList;
        // [SerializeField] private AbilitiesUpgradesList abilitiesUpgradesList;

        private void Start() => Initialize(Model, this);

        public PlayerUpgradeManager Initialize(UnitSpawnData m, PlayerUpgradeManager o) {
            Owner = o;
            Model = m ? m.CreateInstance() : PersistentData.Instance.HordeModel[ControlType.Local].Waves[0].wave[0].CreateInstance();
            statsUpgradesList.Initialize(Model, this);
            // abilitiesUpgradesList.Initialize(Model, this);
            Initialized = true;
            statsUpgradesList.UpdateList();
            return this;
        }

        public bool Initialized { get; private set; }

        public void AddModifier(UnitSpawnData model, UnitModifier modifier) {
            throw new NotImplementedException();
        }

        public void RemoveModifier(UnitSpawnData model, UnitModifier modifier) {
            throw new NotImplementedException();
        }
    }
}