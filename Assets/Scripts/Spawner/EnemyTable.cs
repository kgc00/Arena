using System;
using UnityEngine;

namespace Spawner
{
    [CreateAssetMenu(fileName = "EnemyTable", menuName = "ScriptableObjects/EnemyTable", order = 3), Serializable]
    public class EnemyTable : ScriptableObject
    {
        [SerializeField] public Enemies Enemy;
        [SerializeField] public int Amount;
    }
}