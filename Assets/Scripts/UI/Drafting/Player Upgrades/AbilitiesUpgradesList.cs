using System.Collections.Generic;
using Abilities.Modifiers;
using Common;
using Data.SpawnData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class AbilitiesUpgradesList : 
        MonoBehaviour,
        IInitializable<UnitSpawnData, IModifierHandler<UnitSpawnData, AbilityModifier>, AbilitiesUpgradesList>,
        IModifierHandler<UnitSpawnData, AbilityModifier>  {

        public IModifierHandler<UnitSpawnData, AbilityModifier> Owner { get; private set; }
        public bool Initialized { get; private set; }
        public UnitSpawnData Model { get; private set; }
        public List<AbilityListItem> ListItems { get; protected set; }

        [SerializeField] private GameObject listItem;
        [SerializeField] private GameObject preferredParent;

        public AbilitiesUpgradesList Initialize(UnitSpawnData m, IModifierHandler<UnitSpawnData, AbilityModifier> o) {
            Owner = o;
            Model = m.CreateInstance();
            Initialized = true;
            return this;
        }

        public void AddModifier(UnitSpawnData model, AbilityModifier modifier, int cost) {
            Owner.AddModifier(model, modifier, cost);
        }

        public void RemoveModifier(UnitSpawnData model, AbilityModifier modifier, int cost) {
            Owner.RemoveModifier(model, modifier, cost);
        }
        
        
        private void OnEnable() => UpdateList();

        private void OnDisable() => ClearList();

        public void UpdateList() {
            ClearList();
            CreateList(preferredParent);
        }

        private void CreateList(GameObject preferredParent = null) {
            if (!Initialized) return;

            ListItems = new List<AbilityListItem>();

            AddListItem(Model,new DoubleDamageModifier(null), preferredParent);
            // AddListItem(Model, preferredParent);
            // AddListItem(Model, preferredParent);
        }

        private void ClearList() {
            if (!Initialized || ListItems == null) return;

            ListItems.ForEach(DestroyGameObject);
            ListItems.Clear();
        }

        private void DestroyGameObject(AbilityListItem item) => Destroy(item.gameObject);

        private void AddListItem(UnitSpawnData data, AbilityModifier mod, GameObject preferredParent = null) =>
            ListItems.Add(
                Instantiate(listItem, preferredParent ? preferredParent.transform : gameObject.transform)
                    .GetComponent<AbilityListItem>().Initialize(data, mod, Owner));
    }
}