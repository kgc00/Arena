using System.Collections.Generic;
using Common;
using Data.SpawnData;
using Data.Types;
using UnityEngine;

namespace Data {
    public class ArenaData : MonoBehaviour {
        public Dictionary<ControlType, WaveSpawnData> CurrentWaveModel { get; private set; }
        [SerializeField] private WaveSpawnData playerspawndata;
        [SerializeField] private List<WaveSpawnData> _enemyWaves;
        [SerializeField] private int curIndex;

        public List<WaveSpawnData> EnemyWaves {
            get => _enemyWaves;
            private set => _enemyWaves = value;
        }

        public int CurIndex {
            get => curIndex;
            private set => curIndex = value;
        }

        void Awake() {
            CurrentWaveModel = new Dictionary<ControlType, WaveSpawnData> {
                {ControlType.Local, playerspawndata.CreateInstance()},
                {ControlType.Ai, _enemyWaves[curIndex].CreateInstance()}
            };
        }

        public void IncrementWaveModel() {
            CurIndex++;
            CurrentWaveModel[ControlType.Ai] = EnemyWaves[CurIndex];
        }
    }
}