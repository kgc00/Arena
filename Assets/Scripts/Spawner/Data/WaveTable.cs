using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "WaveTable", menuName = "ScriptableObjects/WaveTable", order = 2), Serializable]
    public class WaveTable : ScriptableObject
    {
        [SerializeField] public List<EnemyTable> Wave;
        [SerializeField] public int WaveNumber;
    }
}