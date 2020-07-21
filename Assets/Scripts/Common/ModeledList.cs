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
        public List<TItem> ListItems { get; protected set; }

        protected virtual void OnEnable() => UpdateList();

        protected virtual void OnDisable() => ClearList();

        public virtual void UpdateModel(TListModel m) {
            Model = m;
            UpdateList();
        }

        public virtual void UpdateList() {
            ClearList();
            CreateList();
        }

        protected virtual void CreateList(GameObject preferredParent = null) {
            if (!Initialized) return;

            ListItems = new List<TItem>();
            Map(Model).ForEach(item => AddListItem(item, preferredParent));
        }

        protected void ClearList() {
            if (!Initialized || ListItems == null) return;

            ListItems.ForEach(DestroyGameObject);
            ListItems.Clear();
        }

        protected void DestroyGameObject(TItem item) => Destroy(item.gameObject);

        protected void AddListItem(TItemModel data, GameObject preferredParent = null) => ListItems.Add(
            Instantiate(listItem, preferredParent ? preferredParent.transform : gameObject.transform)
                .GetComponent<TItem>().Initialize(data, this));

        protected abstract List<TItemModel> Map(TListModel model);
    }
}