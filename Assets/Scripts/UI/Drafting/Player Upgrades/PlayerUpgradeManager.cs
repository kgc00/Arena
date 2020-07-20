using System;
using Common;
using Data.SpawnData;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class PlayerUpgradeManager : MonoBehaviour, IInitializable<UnitData, PlayerUpgradeManager, PlayerUpgradeManager> {
        public PlayerUpgradeManager Owner { get; private set; }
        private UnitData model;

        public UnitData  Model {
            get => model;
            set => model = value;
        }

        [SerializeField] private StatsUpgradesList statsUpgradesList;
        [SerializeField] private AbilitiesUpgradesList abilitiesUpgradesList;

        private void Awake() => Initialize(Model, this);

        public PlayerUpgradeManager Initialize(UnitData m, PlayerUpgradeManager o) {
            
            Owner = o;
            Model = m.CreateInstance();
            statsUpgradesList.Initialize(Model, this);
            abilitiesUpgradesList.Initialize(Model, this);
            Initialized = true;
            return this;
        }

        public bool Initialized { get; private set; }
    }
}