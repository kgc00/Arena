﻿using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace Data.SpawnData
{
    [CreateAssetMenu(fileName = "HordeSpawnData", menuName = "ScriptableObjects/Spawns/HordeSpawnData", order = 1), Serializable]
    public class HordeSpawnData : ScriptableObject
    {
        public List<WaveSpawnData> Waves;

        public void AssignWaveNumbers() {
            for (int i = 0; i < Waves.Count; i++) Waves[i].number = i;
        }
    }
}   