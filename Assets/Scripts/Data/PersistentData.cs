using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.SpawnData;
using Data.Types;
using UnityEngine;

namespace Data {
    public class PersistentData : Singleton<PersistentData> {
        public Dictionary<ControlType, HordeSpawnData> HordeModel { get; private set; }

        [SerializeField] private HordeSpawnData playerspawndata;
        [SerializeField] private HordeSpawnData emptyspawndata;

        protected override void Awake() {
            base.Awake();
            HordeModel = new Dictionary<ControlType, HordeSpawnData> {
                {ControlType.Local, playerspawndata.CreateInstance()},
                {ControlType.Ai, emptyspawndata.CreateInstance()}
            };
        }

        public void UpdateHordeModel(Dictionary<ControlType, HordeSpawnData> update) {
            foreach (var kvp in update) {
                if (HordeModel.ContainsKey(kvp.Key))
                    HordeModel[kvp.Key] = update[kvp.Key];
                else
                    HordeModel.Add(kvp.Key, update[kvp.Key]);
            }
        }
    }
}