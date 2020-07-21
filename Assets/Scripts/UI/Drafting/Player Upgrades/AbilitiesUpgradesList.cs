using System.Collections.Generic;
using Common;
using Data.SpawnData;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilitiesUpgradesList : IInitializable<UnitSpawnData, PlayerUpgradeManager, AbilitiesUpgradesList> {
        public PlayerUpgradeManager Owner { get; }
        public UnitSpawnData Model { get; }
        public AbilitiesUpgradesList Initialize(UnitSpawnData m, PlayerUpgradeManager o) {
            throw new System.NotImplementedException();
        }

        public bool Initialized { get; }
    }
}