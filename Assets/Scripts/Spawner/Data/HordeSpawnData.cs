using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "HordeSpawnData", menuName = "ScriptableObjects/Spawns/HordeSpawnData", order = 1), Serializable]
    public class HordeSpawnData : ScriptableObject
    {
        [SerializeField] public List<WaveSpawnData> Waves;
    }
}   