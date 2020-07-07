using System;
using UnityEngine;
using Types = Units.Types;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "UnitSpawnData", menuName = "ScriptableObjects/Spawns/UnitSpawnData", order = 3), Serializable]
    public class UnitSpawnData : ScriptableObject
    {
        [SerializeField] public Types Unit;
        [SerializeField] public int Amount;
    }
}