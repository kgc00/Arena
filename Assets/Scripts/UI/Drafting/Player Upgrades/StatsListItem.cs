using Common;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class StatsListItem : MonoBehaviour, IInitializable<UnitData, ModeledList<UnitData, UnitData, StatsListItem>, StatsListItem> {
        public ModeledList<UnitData, UnitData, StatsListItem> Owner { get; }
        public UnitData Model { get; }
        public StatsListItem Initialize(UnitData m, ModeledList<UnitData, UnitData, StatsListItem> o) {
            throw new System.NotImplementedException();
        }

        public bool Initialized { get; }
    }
}