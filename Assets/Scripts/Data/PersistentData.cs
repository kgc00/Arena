using Common;
using Data.SpawnData;

namespace Data {
    public class PersistentData : Singleton<PersistentData> {
        public HordeSpawnData HordeModel {
            get;
            private set;
        }

        public void UpdateHordeModel(HordeSpawnData update) => HordeModel = update;
    }
}