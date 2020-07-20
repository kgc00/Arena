using System.Collections.Generic;
using Common;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class StatsUpgradesList : ModeledList<UnitData, UnitData, StatsListItem>,
        IInitializable<UnitData, PlayerUpgradeManager, StatsUpgradesList> {
        protected override List<UnitData> Map(UnitData model) {
            throw new System.NotImplementedException();
        }

        public PlayerUpgradeManager Owner { get; }
        public StatsUpgradesList Initialize(UnitData m, PlayerUpgradeManager o) {
            throw new System.NotImplementedException();
        }
    }
}