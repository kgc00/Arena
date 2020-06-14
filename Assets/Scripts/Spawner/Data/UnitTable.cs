using System;
using UnityEngine;
using Types = Units.Types;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "Unit Table", menuName = "ScriptableObjects/Spawns/Unit Table", order = 3), Serializable]
    public class UnitTable : ScriptableObject
    {
        [SerializeField] public Types Unit;
        [SerializeField] public int Amount;
    }
}