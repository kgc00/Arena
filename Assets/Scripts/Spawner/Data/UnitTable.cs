using System;
using UnityEngine;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "Unit Table", menuName = "ScriptableObjects/Spawns/Unit Table", order = 3), Serializable]
    public class UnitTable : ScriptableObject
    {
        [SerializeField] public Units.Types Unit;
        [SerializeField] public int Amount;
    }
}