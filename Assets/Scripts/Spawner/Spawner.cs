using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Utils;
using  System.Linq;
using Enums;
using Random = UnityEngine.Random;

namespace Spawner
{
    [CreateAssetMenu(fileName = "SpawnTable", menuName = "ScriptableObjects/SpawnTable", order = 1), Serializable]
    public class SpawnTable : ScriptableObject
    {
        [SerializeField] public List<WaveTable> Waves;
    }
    
    [CreateAssetMenu(fileName = "WaveTable", menuName = "ScriptableObjects/WaveTable", order = 2), Serializable]
    public class WaveTable : ScriptableObject
    {
        [SerializeField] public List<EnemyTable> Wave;
        [SerializeField] public int WaveNumber;
    }

    [CreateAssetMenu(fileName = "EnemyTable", menuName = "ScriptableObjects/EnemyTable", order = 3), Serializable]
    public class EnemyTable : ScriptableObject
    {
        [SerializeField] public Enemies Enemy;
        [SerializeField] public int Amount;
    }

    public enum Enemies
    {
        Melee,
        Charging,
        Ranged,
        BombThrowing,
        SuicideBomber,
        Boss
    }

    public enum Intervals
    {
        Timer,
        Wave
    }

    public class Interval
    {
        public Intervals IntervalType { get; } = Intervals.Timer;
    }

    public class WaveHandler
    {
        private SpawnTable spawnTable;
        private WaveTable current;
        private Spawner spawner;

        public WaveHandler(SpawnTable table, Spawner spawner)
        {
            spawnTable = table;
            current =  table.Waves[0];
            this.spawner = spawner;
        }

        private bool ShouldSpawn()
        {
            return true;
        }

        public void Spawn()
        {
            if (!ShouldSpawn()) return;

            HandleSpawn();
        }

        private void HandleSpawn()
        {
            Vector3 spawnerPos = spawner.transform.position;
            Vector3 extentNegative = spawnerPos - spawner.Bounds/2;
            Vector3 extentPositive = spawnerPos + spawner.Bounds/2;
            foreach (EnemyTable enemyTable in current.Wave)
            {
                Debug.Log($"Spawning {enemyTable.Amount} {enemyTable.Enemy}");
                for (int i = 0; i < enemyTable.Amount; i++)
                {
                    var x = Random.Range(extentNegative.x, extentPositive.x);
                    var y = 1.0f;
                    var z = Random.Range(extentNegative.z, extentPositive.z);
                    var spawnPos = new Vector3(x,y,z);
                    spawner.owningPlayer.InstantiateUnit(
                        Resources.Load<GameObject>("Units/Slime/Melee AI"),
                        spawnPos
                        );
                }
            }
        }
    }

    public class Spawner : MonoBehaviour
    {
        [Range(1f, 50f), SerializeField] private float size = 48f;
        public Vector3 Bounds { get; private set; }
        [SerializeField] private Interval interval;
        [SerializeField] private WaveHandler handler;
        public Player owningPlayer { get; private set; }

        private void OnEnable()
        {
            Bounds = new Vector3(size, 1f, size);
            // WILL BREAK IF WE ADD MORE THAN ONE AI PLAYER
            owningPlayer = FindObjectsOfType<Player>().FirstOrDefault(player => player.ControlType == ControlType.Ai);
            handler = new WaveHandler(Resources.Load<SpawnTable>("Data/Spawns/SpawnTable"), this);
            handler.Spawn();
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                transform.position,
                new Vector3(size, 1f, size)
            );
        }
#endif
    }
}