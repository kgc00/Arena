using System.Collections.Generic;
using Common;
using UnityEngine;

namespace UI.Drafting {
    // Owner of child must be this- ergo we use the Modeled List definition in place of the Owner field
    // in the IInitializable type definion of the child.
    public abstract class ModeledList<TListModel, TItemModel, TItem> : MonoBehaviour
        where TListModel : ScriptableObject
        where TItemModel : ScriptableObject
        where TItem : MonoBehaviour,
        IInitializable<TItemModel, ModeledList<TListModel, TItemModel, TItem>, TItem> {
        public TListModel model;

        public TListModel Model {
            get => model;
            protected set => model = value;
        }

        public bool Initialized { get; protected set; }

        [SerializeField] protected GameObject listItem;
        private TItemModel model1;
        public List<TItem> ListItems { get; protected set; }

        protected virtual void OnEnable() => CreateList();

        protected virtual void OnDisable() => ClearList();

        public virtual void UpdateModel(TListModel m) {
            Model = m;
            UpdateList();
        }
        public virtual void UpdateList() {
            ClearList();
            CreateList();
        }

        protected virtual void CreateList() {
            if (!Initialized) return;

            ListItems = new List<TItem>();
            Map(Model).ForEach(AddListItem);
        }

        protected virtual void ClearList() {
            if (!Initialized || ListItems == null) return;

            ListItems.ForEach(RemoveItem);
            ListItems.Clear();
        }

        protected virtual void RemoveItem(TItem item) => Destroy(item.gameObject);

        protected virtual void AddListItem(TItemModel data) =>
            ListItems.Add(Instantiate(listItem, gameObject.transform).GetComponent<TItem>().Initialize(data, this));

        protected abstract List<TItemModel> Map(TListModel model);
    }
}