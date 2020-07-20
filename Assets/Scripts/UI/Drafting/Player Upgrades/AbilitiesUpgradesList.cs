using System.Collections.Generic;
using Common;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilitiesUpgradesList : ModeledList<UnitData, UnitData, StatsListItem>,
        IInitializable<UnitData, PlayerUpgradeManager, AbilitiesUpgradesList> {
        protected override List<UnitData> Map(UnitData model) {
            throw new System.NotImplementedException();
        }

        public PlayerUpgradeManager Owner { get; }
        public AbilitiesUpgradesList Initialize(UnitData m, PlayerUpgradeManager o) {
            throw new System.NotImplementedException();
        }
    }
}