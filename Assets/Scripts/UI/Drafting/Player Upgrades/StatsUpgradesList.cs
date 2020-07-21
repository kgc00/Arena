using System.Collections.Generic;
using Common;
using Data.Modifiers;
using Data.SpawnData;
using Data.Stats;
using Data.Types;
using Data.UnitData;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class StatsUpgradesList :
        MonoBehaviour,
        IInitializable<UnitSpawnData, IModifierHandler<UnitSpawnData, UnitModifier>, StatsUpgradesList>,
        IModifierHandler<UnitSpawnData, UnitModifier> {
        public IModifierHandler<UnitSpawnData, UnitModifier> Owner { get; private set; }

        public bool Initialized { get; private set; }
        public UnitSpawnData Model { get; private set; }
        public List<StatsListItem> ListItems { get; protected set; }

        [SerializeField] protected GameObject listItem;
        [SerializeField] private GameObject preferredParent;

        public StatsUpgradesList Initialize(UnitSpawnData m, IModifierHandler<UnitSpawnData, UnitModifier> o) {
            Owner = o;
            Model = m.CreateInstance();
            Initialized = true;
            return this;
        }

        protected virtual void OnEnable() => UpdateList();

        protected virtual void OnDisable() => ClearList();

        public void AddModifier(UnitSpawnData model, UnitModifier modifier) {
            Owner.AddModifier(model, modifier);
        }

        public void RemoveModifier(UnitSpawnData model, UnitModifier modifier) {
            Owner.RemoveModifier(model, modifier);
        }

        public virtual void UpdateModel(UnitSpawnData m) {
            Model = m;
            UpdateList();
        }

        public virtual void UpdateList() {
            ClearList();
            CreateList();
        }

        protected virtual void CreateList(GameObject preferredParent = null) {
            if (!Initialized) return;

            ListItems = new List<StatsListItem>();
            var stats = new Stats();

            AddListItem(Model, stats.Agility, preferredParent);
            AddListItem(Model, stats.Strength, preferredParent);
            AddListItem(Model, stats.Endurance, preferredParent);
            AddListItem(Model, stats.Intelligence, preferredParent);
            AddListItem(Model, stats.MovementSpeed, preferredParent);
        }

        protected void ClearList() {
            if (!Initialized || ListItems == null) return;

            ListItems.ForEach(DestroyGameObject);
            ListItems.Clear();
        }

        protected void DestroyGameObject(StatsListItem item) => Destroy(item.gameObject);

        protected void AddListItem(UnitSpawnData data, Statistic stat, GameObject preferredParent = null) =>
            ListItems.Add(
                Instantiate(listItem, preferredParent ? preferredParent.transform : gameObject.transform)
                    .GetComponent<StatsListItem>().Initialize(data, stat, this));
    }
}