using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner.Data
{
    [CreateAssetMenu(fileName = "SpawnTable", menuName = "ScriptableObjects/SpawnTable", order = 1), Serializable]
    public class SpawnTable : ScriptableObject
    {
        [SerializeField] public List<WaveTable> Waves;
    }
}    