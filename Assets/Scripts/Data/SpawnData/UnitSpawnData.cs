using System;
using System.Collections.Generic;
using Data.Modifiers;
using Data.Types;
using UnityEngine;

namespace Data.SpawnData
{
    [CreateAssetMenu(fileName = "UnitSpawnData", menuName = "ScriptableObjects/Spawns/UnitSpawnData", order = 3), Serializable]
    public class UnitSpawnData : ScriptableObject
    {
        [SerializeField] public UnitType Unit;
        [SerializeField] public int Amount;
        [SerializeField] public List<UnitModifierType> modifiers;
    }
}