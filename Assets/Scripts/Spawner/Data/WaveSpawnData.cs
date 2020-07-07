using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "WaveSpawnData", menuName = "ScriptableObjects/Spawns/WaveSpawnData", order = 2), Serializable]
    public class WaveSpawnData : ScriptableObject
    {
        [SerializeField] public List<UnitSpawnData> wave;
        [SerializeField] public int number;
    }
}