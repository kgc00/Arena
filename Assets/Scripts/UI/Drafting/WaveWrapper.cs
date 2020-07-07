using System;
using System.Collections.Generic;
using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public class WaveWrapper : MonoBehaviour {
        public WaveSpawnData model;
        public WaveSpawnData Model {
            get => model;
            private set => model = value;
        }
        [SerializeField] private GameObject listItem;
        public List<VisualizerListItem> ListItems { get; private set; }

        private void OnEnable() => CreateList();

        private void OnDisable() => ClearList();

        void UpdateList() {
            ClearList();
            CreateList();
        }

        private void CreateList() {
            ListItems = new List<VisualizerListItem>();
            Model.wave.ForEach(AddListItem);
        }

        private void ClearList() {
            ListItems.ForEach(RemoveItem);
            ListItems.Clear();
        }

        private void RemoveItem(VisualizerListItem item) => Destroy(item.gameObject);

        private void AddListItem(UnitSpawnData unitSpawnData) => ListItems.Add(Instantiate(listItem, gameObject.transform).GetComponent<VisualizerListItem>().Initialize(unitSpawnData));
    }
}