using Common;
using Data.SpawnData;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilityListItem : MonoBehaviour, IInitializable<UnitSpawnData, ModeledList<UnitSpawnData, UnitSpawnData, AbilityListItem>, AbilityListItem> {
        public ModeledList<UnitSpawnData, UnitSpawnData, AbilityListItem> Owner { get; }
        public UnitSpawnData Model { get; }

        public AbilityListItem Initialize(UnitSpawnData m, ModeledList<UnitSpawnData, UnitSpawnData, AbilityListItem> o) {
            throw new System.NotImplementedException();
        }

        public bool Initialized { get; }
    }
}