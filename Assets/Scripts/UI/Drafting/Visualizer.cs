using Spawner.Data;
using UnityEngine;

namespace UI.Drafting {
    public class Visualizer : MonoBehaviour {
        public HordeSpawnData model;
        public HordeSpawnData Model {
            get => model;
            set => model = value;
        }

        public Visualizer Initialize(HordeSpawnData st) {
            Model = st.CreateInstance();
            return this;
        }
    }
}