using System.Collections;
using System.Collections.Generic;
using Common;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public abstract class ModeledList<TListModel, TObjModel, TObj> : MonoBehaviour
        where TListModel : ScriptableObject
        where TObj : MonoBehaviour, IInitializable<TObjModel, TObj>
        where TObjModel : ScriptableObject {
        public TListModel model;

        public TListModel Model {
            get => model;
            private set => model = value;
        }

        [SerializeField] protected GameObject listItem;
        public List<TObj> ListItems { get; protected set; }

        protected virtual void OnEnable() => CreateList();

        protected virtual void OnDisable() => ClearList();

        protected virtual void UpdateList() {
            ClearList();
            CreateList();
        }

        protected virtual void CreateList() {
            ListItems = new List<TObj>();
            Map(Model).ForEach(AddListItem);
        }

        protected virtual void ClearList() {
            ListItems.ForEach(RemoveItem);
            ListItems.Clear();
        }

        protected virtual void RemoveItem(TObj item) => Destroy(item.gameObject);

        protected virtual void AddListItem(TObjModel data) =>
            ListItems.Add(Instantiate(listItem, gameObject.transform).GetComponent<TObj>().Initialize(data));

        protected abstract List<TObjModel> Map(TListModel model);
    }
}