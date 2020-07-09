using System;
using Data.SpawnData;
using TMPro;
using UnityEngine;
using Common;

namespace UI.Drafting {
    public class WaveButton : MonoBehaviour,
        IInitializable<WaveSpawnData, ModeledList<HordeSpawnData, WaveSpawnData, WaveButton>, WaveButton> {
        private string waveText;
        public TextMeshProUGUI WaveUgui { get; set; }
        public ModeledList<HordeSpawnData, WaveSpawnData, WaveButton> Owner { get; private set; }
        public WaveSpawnData Model { get; private set; }
        public bool Initialized { get; private set; }
        public static Action<int> OnWaveButtonClick = delegate {  };

        private void Awake() {
            WaveUgui = transform.Find("Wave Text").GetComponent<TextMeshProUGUI>() ??
                       throw new Exception("Unable to find wave UI element");
        }

        public WaveButton Initialize(WaveSpawnData m,
            ModeledList<HordeSpawnData, WaveSpawnData, WaveButton> o) {
            Owner = o;
            Model = m;
            waveText = Model.number.ToString();
            WaveUgui.SetText(waveText);
            Initialized = true;
            return this;
        }

        public void HandleClick() => OnWaveButtonClick(Model.number);
    }
}