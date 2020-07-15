using System;
using System.Collections.Generic;
using Data.Modifiers;
using TypeReferences;
using UnityEngine;

namespace Data.SpawnData
{
    [CreateAssetMenu(fileName = "WaveSpawnData", menuName = "ScriptableObjects/Spawns/WaveSpawnData", order = 2), Serializable]
    public class WaveSpawnData : ScriptableObject
    {
        [SerializeField] public List<UnitSpawnData> wave;
        [HideInInspector] public int number;
        [SerializeField, ClassExtends(typeof(WaveModifier))] public List<ClassTypeReference> modifiers;
    }
}