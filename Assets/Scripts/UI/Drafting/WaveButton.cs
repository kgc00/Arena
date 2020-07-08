using System;
using Common;
using Spawner.Data;
using TMPro;
using UnityEngine;

namespace UI.Drafting {
    public class WaveButton : MonoBehaviour, IInitializable<WaveSpawnData, WaveButton> {
        public TextMeshProUGUI WaveUgui {get; set; }
        private string waveText;
        public WaveSpawnData Model { get; private set; }
        public bool Initialized { get; private set; }


        private void Awake() {
            WaveUgui = transform.Find("Wave Text").GetComponent<TextMeshProUGUI>() ??
                       throw new Exception("Unable to find wave UI element");
        }


        public WaveButton Initialize(WaveSpawnData modelWave) {
            Model = modelWave;
            waveText = Model.number.ToString();
            WaveUgui.SetText(waveText);
            Initialized = true;
            return this;
        }

    }
}