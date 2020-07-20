using Common;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilityListItem : MonoBehaviour, IInitializable<UnitData, ModeledList<UnitData, UnitData, AbilityListItem>, AbilityListItem> {
        public ModeledList<UnitData, UnitData, AbilityListItem> Owner { get; }
        public UnitData Model { get; }
        public AbilityListItem Initialize(UnitData m, ModeledList<UnitData, UnitData, AbilityListItem> o) {
            throw new System.NotImplementedException();
        }

        public bool Initialized { get; }
    }
}