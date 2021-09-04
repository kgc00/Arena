using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.SpawnData;
using Data.Types;
using UnityEngine;

namespace Data {
    public class PersistentData : Singleton<PersistentData> {
        public Dictionary<ControlType, HordeSpawnData> CurrentHordeModel { get; private set; }
        public float currency;

        [SerializeField] private HordeSpawnData playerspawndata;
        // [SerializeField] private HordeSpawnData emptyspawndata; // debugging
        [SerializeField] private List<HordeSpawnData> enemyHordes;
        [SerializeField] private int curIndex;

        public List<HordeSpawnData> EnemyHordes {
            get => enemyHordes;
            private set => enemyHordes = value;
        }

        public int CurIndex {
            get => curIndex;
            private set => curIndex = value;
        }

        protected override void Awake() {
            base.Awake();
            CurrentHordeModel = new Dictionary<ControlType, HordeSpawnData> {
                {ControlType.Local, playerspawndata.CreateInstance()},
                {ControlType.Ai, enemyHordes[curIndex].CreateInstance()}
            };
        }

        private void Start() {
            
        }

        public void UpdateHordeModel(ControlType ctrlType, HordeSpawnData horde) {
            foreach (var spawnData in CurrentHordeModel[ctrlType].Waves[0].wave) {
                print(spawnData.Unit);
            }
            
            foreach (var spawnData in horde.Waves[0].wave) {
                print(spawnData.Unit);
            }
            
            CurrentHordeModel[ctrlType] = horde;
            
            foreach (var spawnData in CurrentHordeModel[ctrlType].Waves[0].wave) {
                print(spawnData.Unit);
            }
        }

        public void IncrementHordeModel() {
            print("increment called");
            CurIndex++;
            CurrentHordeModel[ControlType.Ai] = EnemyHordes[CurIndex];
        }
    }
}