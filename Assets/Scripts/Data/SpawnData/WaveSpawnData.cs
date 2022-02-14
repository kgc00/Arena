using System;
using System.Collections.Generic;
using Data.Modifiers;
using UnityEngine;

namespace Data.SpawnData
{
    [CreateAssetMenu(fileName = "WaveSpawnData", menuName = "ScriptableObjects/Spawns/WaveSpawnData", order = 2), Serializable]
    public class WaveSpawnData : ScriptableObject
    {
        [SerializeField] public List<UnitSpawnData> wave;
        [SerializeField] public List<WaveModifierType> modifiers;
    }
}