using Common;
using Data.SpawnData;
using UnityEngine;

namespace Data {
    public class PersistentData : Singleton<PersistentData> {
        public HordeSpawnData HordeModel {
            get;
            private set;
        }

        [SerializeField] public int id; 

        public void UpdateHordeModel(HordeSpawnData update) => HordeModel = update;
    }
}