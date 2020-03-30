using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "Wave Table", menuName = "ScriptableObjects/Spawns/Wave Table", order = 2), Serializable]
    public class WaveTable : ScriptableObject
    {
        [SerializeField] public List<UnitTable> Wave;
        [SerializeField] public int WaveNumber;
    }
}