using System.Collections.Generic;
using Common;
using Data.SpawnData;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilitiesUpgradesList : ModeledList<UnitSpawnData, UnitSpawnData, StatsListItem>,
        IInitializable<UnitSpawnData, PlayerUpgradeManager, AbilitiesUpgradesList> {
        protected override List<UnitSpawnData> Map(UnitSpawnData model) {
            throw new System.NotImplementedException();
        }

        public PlayerUpgradeManager Owner { get; }
        public AbilitiesUpgradesList Initialize(UnitSpawnData m, PlayerUpgradeManager o) {
            throw new System.NotImplementedException();
        }
    }
}