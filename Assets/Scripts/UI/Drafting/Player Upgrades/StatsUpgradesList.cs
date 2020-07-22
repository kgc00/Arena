using System.Collections.Generic;
using Common;
using Data.Modifiers;
using Data.SpawnData;
using Data.Stats;
using Data.Types;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public sealed class StatsUpgradesList :
        MonoBehaviour,
        IInitializable<UnitSpawnData, IModifierHandler<UnitSpawnData, UnitModifier>, StatsUpgradesList>,
        IModifierHandler<UnitSpawnData, UnitModifier> {
        public IModifierHandler<UnitSpawnData, UnitModifier> Owner { get; private set; }

        public bool Initialized { get; private set; }
        public UnitSpawnData Model { get; private set; }
        public List<StatsListItem> ListItems { get; protected set; }

        [SerializeField] private GameObject listItem;
        [SerializeField] private GameObject preferredParent;

        public StatsUpgradesList Initialize(UnitSpawnData m, IModifierHandler<UnitSpawnData, UnitModifier> o) {
            Owner = o;
            Model = m.CreateInstance();
            Initialized = true;
            return this;
        }

        private void OnEnable() => UpdateList();

        private void OnDisable() => ClearList();

        public void AddModifier(UnitSpawnData model, UnitModifier modifier, int cost) {
            Owner.AddModifier(model, modifier, cost);
        }

        public void RemoveModifier(UnitSpawnData model, UnitModifier modifier, int cost) {
            Owner.RemoveModifier(model, modifier, cost);
        }

        public void UpdateList() {
            ClearList();
            CreateList(preferredParent);
        }

        private void CreateList(GameObject preferredParent = null) {
            if (!Initialized) return;

            ListItems = new List<StatsListItem>();
            var stats = new Stats();

            AddListItem(Model, stats.Strength, preferredParent);
            AddListItem(Model, stats.Endurance, preferredParent);
            AddListItem(Model, stats.MovementSpeed, preferredParent);
        }

        private void ClearList() {
            if (!Initialized || ListItems == null) return;

            ListItems.ForEach(DestroyGameObject);
            ListItems.Clear();
        }

        private void DestroyGameObject(StatsListItem item) => Destroy(item.gameObject);

        private void AddListItem(UnitSpawnData data, Statistic stat, GameObject preferredParent = null) =>
            ListItems.Add(
                Instantiate(listItem, preferredParent ? preferredParent.transform : gameObject.transform)
                    .GetComponent<StatsListItem>().Initialize(data, stat, this));
    }
}