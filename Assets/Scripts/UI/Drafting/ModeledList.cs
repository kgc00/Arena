using System.Collections;
using System.Collections.Generic;
using Common;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public abstract class ModeledList<TListModel, TObjModel, TObj> : MonoBehaviour,
        IInitializable<TListModel, ModeledList<TListModel, TObjModel, TObj>>
        where TListModel : ScriptableObject
        where TObj : MonoBehaviour, IInitializable<TObjModel, TObj>
        where TObjModel : ScriptableObject {
        public TListModel model;

        public TListModel Model {
            get => model;
            protected set => model = value;
        }

        public virtual ModeledList<TListModel, TObjModel, TObj> Initialize(TListModel model) {
            Model = model;
            Initialized = true;
            return this;
        }

        public bool Initialized { get; protected set; }

        [SerializeField] protected GameObject listItem;
        private TObjModel model1;
        public List<TObj> ListItems { get; protected set; }

        protected virtual void OnEnable() => CreateList();

        protected virtual void OnDisable() => ClearList();

        public virtual void UpdateList() {
            ClearList();
            CreateList();
        }

        protected virtual void CreateList() {
            if (!Initialized) return;
            
            ListItems = new List<TObj>();
            Map(Model).ForEach(AddListItem);
        }

        protected virtual void ClearList() {
            if (!Initialized || ListItems == null) return;

            ListItems.ForEach(RemoveItem);
            ListItems.Clear();
        }

        protected virtual void RemoveItem(TObj item) => Destroy(item.gameObject);

        protected virtual void AddListItem(TObjModel data) =>
            ListItems.Add(Instantiate(listItem, gameObject.transform).GetComponent<TObj>().Initialize(data));

        protected abstract List<TObjModel> Map(TListModel model);
    }
}